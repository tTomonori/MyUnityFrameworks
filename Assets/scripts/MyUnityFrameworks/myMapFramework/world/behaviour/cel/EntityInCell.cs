using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInCell : MapEntity {
    [SerializeField] private MapStandImage _Image;
    public override MapStandImage mImage {
        get { return _Image; }

        set { _Image = value; }
    }
}
