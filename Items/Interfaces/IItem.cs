using UnityEngine;
using System.Collections;

public interface IItem {
    int ID { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    ushort Image { get; set; }
    int Value { get; set; }
    Texture2D Icon { get; set; }
    float Weight { get; set; }
    Quality Quality { get; set; }
    bool Tradeable { get; set; }
}
