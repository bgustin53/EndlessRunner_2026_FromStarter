/************************************************
* Attached to the VisibilityTester
* Test visibility of various access modifiers
* Author: BGustin
* Version 11/01/2026v1.0
************************************************/

using UnityEngine;
using System.Reflection;
using System;

public class VisibilityTester : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private MonoBehaviour gameManagerScript; // Drag GameManager component here
    [SerializeField] private MonoBehaviour uiManagerScript;   // Drag UIManager component here

    void Start()
    {
        Debug.Log("--- STARTING ACCESSIBILITY AUDIT ---");

        // 1. Test GameManager Fields & Properties
        if (gameManagerScript != null)
        {
            Type gmType = gameManagerScript.GetType();

            // Test Property: Instance
            TestPropertyAccess(gmType, "Instance");
            
            // Test Field: speed
            TestFieldAccess(gmType, "speed");

            // Test Property: startGame
            TestPropertyAccess(gmType, "startGame");
        }
        else
        {
            Debug.LogError("[Test Setup] GameManager Script component reference is missing!");
        }

        // 2. Test UIManager Fields & Properties
        if (uiManagerScript != null)
        {
            Type uiType = uiManagerScript.GetType();

            // Test Field: score
            TestFieldAccess(uiType, "score");

            // Test Field: UIDocument
            TestFieldAccess(uiType, "UIDocument");
        }
        else
        {
            Debug.LogError("[Test Setup] UIManager Script component reference is missing!");
        }

        Debug.Log("--- ACCESSIBILITY AUDIT COMPLETE ---");
    }

    /// <summary>
    /// Checks whether a Field provides public external access.
    /// </summary>
    private void TestFieldAccess(Type targetType, string fieldName)
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        FieldInfo field = targetType.GetField(fieldName, flags);

        if (field == null)
        {
            Debug.LogError($"<color=red>[FAIL]</color> Field '{fieldName}' was not visible to GET or SET on '{targetType.Name}'.");
            return;
        }

        if (field.IsPublic)
        {
            Debug.Log($"<color=green>[PASS]</color> Field '{fieldName}' on '{targetType.Name}' allows External access to GET and SET.");
        }
        else
        {
            Debug.LogError($"<color=red>[FAIL]</color> External Access Denied: Field '{fieldName}' on '{targetType.Name}' is PRIVATE or NON-PUBLIC.");
        }
    }

    /// <summary>
    /// Checks whether a Property provides public external getter access.
    /// </summary>
    private void TestPropertyAccess(Type targetType, string propertyName)
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        PropertyInfo property = targetType.GetProperty(propertyName, flags);

        if (property == null)
        {
            Debug.LogError($"<color=red>[FAIL]</color> Property '{propertyName}' was not found on '{targetType.Name}'.");
            return;
        }

        // Retrieve public accessors (returns null if accessor is non-public or missing)
        MethodInfo getMethod = property.GetGetMethod(nonPublic: false);
        MethodInfo setMethod = property.GetSetMethod(nonPublic: false);

        bool canGet = getMethod != null && getMethod.IsPublic;
        bool canSet = setMethod != null && setMethod.IsPublic;

        // 1. Evaluate Getter (Read Access)
        if (canGet)
        {
            Debug.Log($"<color=green>[PASS]</color> Property '{propertyName}' on '{targetType.Name}' allows External access to GET.");
        }
        else
        {
            Debug.LogError($"<color=red>[FAIL]</color> External Read Access Denied: Property '{propertyName}' on '{targetType.Name}' does not allows External access to GET.");
        }

        // 2. Evaluate Setter (Write Access)
        if (canSet)
        {
            Debug.Log($"<color=green>[PASS]</color> Property '{propertyName}' on '{targetType.Name}' allows External access to SET.");
        }
        else
        {
            Debug.LogError($"<color=red>[FAIL]</color> External Write Access Denied: Property '{propertyName}' on '{targetType.Name}' does not allows External access to SET.");
        }
    }
}
