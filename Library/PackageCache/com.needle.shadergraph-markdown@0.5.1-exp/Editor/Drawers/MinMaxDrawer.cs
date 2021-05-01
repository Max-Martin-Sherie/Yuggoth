using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Needle.ShaderGraphMarkdown;
using UnityEditor;
using UnityEngine;

namespace Needle.ShaderGraphMarkdown
{
    public class MinMaxDrawer : MarkdownMaterialPropertyDrawer
    {
        float GetValue(MaterialProperty property, char swizzle)
        {
            switch (property.type)
            {
                case MaterialProperty.PropType.Float:
                case MaterialProperty.PropType.Range:
                    return property.floatValue;
                case MaterialProperty.PropType.Vector:
                    return GetValue(property.vectorValue, swizzle);
                default:
                    return 0;
            }
        }

        void SetValue(MaterialProperty property, char swizzle, float value)
        {
            switch (property.type)
            {
                case MaterialProperty.PropType.Float:
                case MaterialProperty.PropType.Range:
                    property.floatValue = value;
                    break;
                case MaterialProperty.PropType.Vector:
                    var val = property.vectorValue;
                    SetValue(ref val, swizzle, value);
                    property.vectorValue = val;
                    break;
                default:
                    return;
            }
        }
        
        float GetValue(Vector4 vector, char swizzle)
        {
            switch (swizzle)
            {
                case 'x': case 'r': return vector.x;
                case 'y': case 'g': return vector.y;
                case 'z': case 'b': return vector.z;
                case 'w': case 'a': return vector.w;
                default: return 0;
            }
        }

        void SetValue(ref Vector4 vector, char swizzle, float value)
        {
            switch (swizzle)
            {
                case 'x': case 'r': vector.x = value; return;
                case 'y': case 'g': vector.y = value; return;
                case 'z': case 'b': vector.z = value; return;
                case 'w': case 'a': vector.w = value; return;
                default: return;
            }
        }

        void GetPropertyNameAndSwizzle(string parameterName, out string propertyName, out char swizzle)
        {
            var indexOfDot = parameterName.LastIndexOf('.');
            if (indexOfDot > 0 && indexOfDot == parameterName.Length - 2)
            {
                swizzle = parameterName[parameterName.Length - 1];
                propertyName = parameterName.Substring(0, parameterName.Length - 2);
            }
            else
            {
                swizzle = ' ';
                propertyName = parameterName;
            }
        }
        
        public override void OnDrawerGUI(MaterialEditor materialEditor, MaterialProperty[] properties, DrawerParameters parameters)
        {
            // check if these are vectors, and get the "base property"
            var parameterName1 = parameters.Get(0, (string) null);
            var parameterName2 = parameters.Get(1, (string) null);

            if (parameterName2 == null)
            {
                // parameter 1 must be a vector, no swizzles
                // we're using zw as limits
                var vectorProp = properties.FirstOrDefault(x => x.name.Equals(parameterName1, StringComparison.Ordinal));
                if (vectorProp == null || vectorProp.type != MaterialProperty.PropType.Vector)
                {
                    EditorGUILayout.HelpBox(nameof(MinMaxDrawer) + ": Parameter is not a Vector property (" + parameterName1 + ")", MessageType.Error);
                    return;
                }

                EditorGUI.showMixedValue = vectorProp.hasMixedValue;
                var vec = vectorProp.vectorValue;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.MinMaxSlider(vectorProp.displayName, ref vec.x, ref vec.y, vec.z, vec.w);
                if (EditorGUI.EndChangeCheck())
                {
                    vectorProp.vectorValue = vec;
                }

                EditorGUI.showMixedValue = false;
                return;
            }

            if (parameterName1 == null || parameterName2 == null) {
                EditorGUILayout.HelpBox(nameof(MinMaxDrawer) + ": Parameter names are incorrect (" + parameterName1 + ", " + parameterName2 + "), all: " + string.Join(",", parameters), MessageType.Error);
                return;
            }
            
            GetPropertyNameAndSwizzle(parameterName1, out var propertyName1, out var swizzle1);
            GetPropertyNameAndSwizzle(parameterName2, out var propertyName2, out var swizzle2);
            
            var param1 = properties.FirstOrDefault(x => x.name.Equals(propertyName1, StringComparison.Ordinal));
            var param2 = properties.FirstOrDefault(x => x.name.Equals(propertyName2, StringComparison.Ordinal));

            if (param1 == null || param2 == null) {
                EditorGUILayout.HelpBox(nameof(MinMaxDrawer) + ": Parameter names are incorrect (" + propertyName1 + ", " + propertyName2 + "), all: " + string.Join(",", parameters), MessageType.Error);
                return;
            }
            
            float value1 = GetValue(param1, swizzle1);
            float value2 = GetValue(param2, swizzle2);

            var display = param1 == param2 ? (param1.displayName + " (" + swizzle1 + swizzle2 + ")") : param1.displayName + " - " + param2.displayName;
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.MinMaxSlider(display, ref value1, ref value2, 0.0f, 1.0f);
            if (EditorGUI.EndChangeCheck())
            {
                SetValue(param1, swizzle1, value1);
                SetValue(param2, swizzle2, value2);
            }
        }

        public override IEnumerable<MaterialProperty> GetReferencedProperties(MaterialEditor materialEditor, MaterialProperty[] properties, DrawerParameters parameters)
        {
            var parameterName1 = parameters.Get(0, (string) null);
            var parameterName2 = parameters.Get(1, (string) null);

            if (parameterName1 == null || parameterName2 == null)
                return null;
            
            GetPropertyNameAndSwizzle(parameterName1, out var propertyName1, out var swizzle1);
            GetPropertyNameAndSwizzle(parameterName2, out var propertyName2, out var swizzle2);
            
            var param1 = properties.FirstOrDefault(x => x.name.Equals(propertyName1, StringComparison.Ordinal));
            var param2 = properties.FirstOrDefault(x => x.name.Equals(propertyName2, StringComparison.Ordinal));
            
            if (param1 == null || param2 == null) return null;
            return new[] { param1, param2 };
        }
    }
}