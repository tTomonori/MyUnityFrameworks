Shader "My/Cast"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,0.3)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _CoefficientX ("CoefficientX", Vector) = (1,0,0,0)
        _CoefficientY ("CoefficientY", Vector) = (0,1,1,0)
    }
    SubShader
    {

        Tags { "Queue"="Transparent+1" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
        ZTest Greater
        Cull Front
        Stencil {
          Ref 1
          Comp always
          Pass replace
        }
        Pass{
            ZWrite OFF
            ColorMask 0
        }
        
        ZTest Less
        Cull Back
        Stencil {
          Ref 1
          Comp Equal
          Pass Zero
          ZFail Zero
        }

        Pass
        {
        
           CGPROGRAM
           // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members v)
           #pragma exclude_renderers d3d11
           #pragma vertex vert
           #pragma fragment frag
            
           #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 projPos : TEXCOORD2;
                float4 v:TEXCOORD3;// = v.vertex(頂点のローカル座標)
            };
            
            sampler2D _CameraDepthTexture;
            float4 _CameraDepthTexture_ST;
            fixed4 _Color;
            sampler2D _MainTex;
            float4 _CoefficientX;
            float4 _CoefficientY;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.v=v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _CameraDepthTexture);
                o.projPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                //カメラからの距離(このobj)
                fixed myDepth = i.projPos.z;
                //0~1線形(描画済み)
                fixed l = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,i.projPos));
                //カメラからの距離(描画済み)
                fixed drawnDepth = _ProjectionParams.y+l*(_ProjectionParams.z-_ProjectionParams.y);
                //ローカル座標系でのカメラの向き
                float3 eyeDir = -UNITY_MATRIX_V[2].xyz;
                //描画済座標のローカル座標
                float4 drawnLocal= i.v+float4(eyeDir.x,eyeDir.y,eyeDir.z,0)*(drawnDepth-myDepth);
                
                float2 uv = float2(drawnLocal.x*_CoefficientX.x+drawnLocal.y*_CoefficientX.y+drawnLocal.z*_CoefficientX.z+_CoefficientX.w,
                            drawnLocal.x*_CoefficientY.x+drawnLocal.y*_CoefficientY.y+drawnLocal.z*_CoefficientY.z+_CoefficientY.w);
                              
                fixed4 col = tex2D(_MainTex, uv);
                clip(col.a - 0.5);
                col=col*_Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
/*
  Camera.depthTextureMode |= DepthTextureMode.Depth;
  が必要
  
  影を受ける側のshaderは
  FallBack "Diffuse"
  が必要(↑がないshaderと重なった場合は無効になる)
*/