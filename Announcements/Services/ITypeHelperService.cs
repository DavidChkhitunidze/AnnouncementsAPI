﻿namespace AnnouncementsAPI.Services
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
