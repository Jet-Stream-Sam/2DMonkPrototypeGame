using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetting
{
    string SettingName { get; set; }
    void SetChanges(object value);
}
