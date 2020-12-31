/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace Factory.Lib
{
    public static class TypeBuilder
    {
        public static T CreateType<T>(string typeName, bool isSingleton = false)
        {
            T instance = default(T);

            if (typeName == null)
            {
                throw new Exception("TypeName cannot be null.");
            }

            Type type = Type.GetType(typeName, true);

            if (type == null)
            {
                throw new Exception(string.Format("Unable to load type {0}", typeName));
            }

            if (isSingleton)
            {
                var info = type.GetProperty("Instance");
                instance = (T) info.GetValue(null, null);
            }
            else
            {
                // alternative
                //IEnumerable<ConstructorInfo> constructorsInfo = type.GetConstructors();
                //ConstructorInfo constructorInfo = constructorsInfo.Single(x => x.GetParameters().Count() == 0);
                //instance = (T)constructorInfo.Invoke(null);

                instance = (T) Activator.CreateInstance(type, null);
            }

            return instance;
        }
    }
}