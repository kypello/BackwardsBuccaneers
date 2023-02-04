using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    string prompt { get; }

    IEnumerator Interact();
}
