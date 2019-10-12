﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageShadowTrigger : ImageEventTrigger {
    public float mShadePower = 0.3f;
    public override void plusEvent(ImageEventData aData, MapStandBehaviour aBehaviour) {
        aData.shade(mShadePower);
    }
}
