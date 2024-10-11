using UnityEditor;
using RCore.Common;

namespace RCore.Inspector.Editor
{
    [CustomPropertyDrawer(typeof(StringStringDictionary))]
    [CustomPropertyDrawer(typeof(ObjectColorDictionary))]
    [CustomPropertyDrawer(typeof(ObjectObjectDictionary))]
    public class SerializableDictionaryDrawer : SerializableDictionaryPropertyDrawer { }
}