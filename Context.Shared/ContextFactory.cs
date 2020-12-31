/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;

namespace Context.Shared
{
    public class ContextFactory
    {
        private static Dictionary<UniqueContextType, Context> _contexts = new Dictionary<UniqueContextType, Context>();
        private static object _contextsLock = new object();

        public static Context GetContext(UniqueContextType type)
        {
            if (!_contexts.ContainsKey(type))
            {
                lock (_contextsLock)
                {
                    if (!_contexts.ContainsKey(type))
                    {
                        _contexts[type] = new Context(type);
                    }
                }
            }

            return _contexts[type];
        }

        public static object GetSubcontext(Context context, ContextType contextType)
        {
            //context.Subcontexts.
            throw new NotImplementedException();
        }
    }
}