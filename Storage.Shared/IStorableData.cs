/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace Storage.Shared
{
    public interface IStorableData
    {
        byte[] GetBytes();

        void SetBytes(byte[] bytes);
    }
}