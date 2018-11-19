using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;
using System.Reflection;

public static class Tool
{
    public static string GetColor(string text, string type)
    {
        return  "<color=" + type + ">" + text + "</color>";

    }


    public static int rd
    {
        get
        {
            Random r = new Random(GetRandomSeed());
            return r.Next(0, 101);
        }
    }

    public static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    public static string GetRS(string text)
    {
        string[] s = text.Split('|');
        Random r = new Random();
        return s[r.Next(0,s.Length - 1)];
    }


    public static T DeepCopy2<T>(T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", "source");
        }

        // Don't serialize a null object, simply return the default for that object
        if (Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using (stream)
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }
    public static object DeepCopy(object srcobj)
    {
        if (srcobj == null)
        {
            return null;
        }

        Type srcObjType = srcobj.GetType();

        // Is simple value type, directly assign  
        if (srcObjType.IsValueType)
        {
            return srcobj;
        }
        // Is array  
        if (srcObjType.IsArray)
        {
            return DeepCopyArray(srcobj as Array);
        }
        // is List or map  
        else if (srcObjType.IsGenericType)
        {
            return DeepCopyGenericType(srcobj);
        }
        // is cloneable  
        else if (srcobj is ICloneable)
        {
            // Log informations  
            return (srcobj as ICloneable).Clone();
        }
        else
        {
            // Try to do deep copy, create a new copied instance  
            object deepCopiedObj = System.Activator.CreateInstance(srcObjType);

            // Find out all fields or properties, do deep copy  
            BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.Instance;
            MemberInfo[] memberCollection = srcObjType.GetMembers(bflags);

            foreach (MemberInfo member in memberCollection)
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    object fieldValue = field.GetValue(srcobj);
                    field.SetValue(deepCopiedObj, DeepCopy(fieldValue));
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = (PropertyInfo)member;
                    MethodInfo info = property.GetSetMethod(false);
                    if (info != null)
                    {
                        object propertyValue = property.GetValue(srcobj, null);
                        property.SetValue(deepCopiedObj, DeepCopy(propertyValue), null);
                    }
                }
            }

            return deepCopiedObj;
        }
    }
    private static Array DeepCopyArray(Array srcArray)
    {
        if (srcArray.Length <= 0)
        {
            return null;
        }
        // Create new array instance based on source array  
        Array arrayCopied = Array.CreateInstance(srcArray.GetValue(0).GetType(), srcArray.Length);
        // deep copy each object in array  
        for (int i = 0; i < srcArray.Length; i++)
        {
            object o = DeepCopy(srcArray.GetValue(i));
            arrayCopied.SetValue(o, i);
        }
        return arrayCopied;
    }

    private static object DeepCopyGenericType(object srcGeneric)
    {
        try
        {
            // Is List   
            IList srcList = srcGeneric as IList;
            if (srcList.Count <= 0)
            {
                return null;
            }

            // Create new List<object> instance  
            IList dstList = Activator.CreateInstance(srcList.GetType()) as IList;
            // deep copy each object in List  
            foreach (object o in srcList)
            {
                dstList.Add(DeepCopy(o));
            }

            return dstList;
        }
        catch (Exception)
        {
            try
            {
                IDictionary srcDictionary = srcGeneric as IDictionary;
                if (srcDictionary.Count <= 0)
                {
                    return null;
                }

                // Create new map instance  
                IDictionary dstDictionary = Activator.CreateInstance(srcDictionary.GetType()) as IDictionary;
                // deep copy each object in map  
                foreach (object o in srcDictionary.Keys)
                {
                    dstDictionary[o] = srcDictionary[o];
                }
                return dstDictionary;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }


    #region tools

    public static byte[] SerializeObj(Object obj)
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, obj);
        byte[] data = ms.ToArray();
        return data;
    }


    public static T DeSerializeObj<T>(byte[] data)
    {
        MemoryStream ms1 = new MemoryStream(data);
        BinaryFormatter bf1 = new BinaryFormatter();
        T type = (T)bf1.Deserialize(ms1);
        return type;
    }


    #endregion







    public static int[] GetRamdomNums(int howmuch)
    {
        int[] nums = new int[howmuch];
        for (int i = 0; i < howmuch; i++)
        {
            nums[i] = i;
        }
        return nums.OrderBy(x => Guid.NewGuid()).ToArray();
    }
}
