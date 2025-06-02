using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
public static class TypeExtensions
{
    public static bool IsAssignableFromGenericType(this Type givenType, Type genericType)
    {
        return givenType.GetInterfaces().Any(t =>
            t.IsGenericType && t.GetGenericTypeDefinition() == genericType);
    }
}