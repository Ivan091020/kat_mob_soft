using AutoMapper;
using kat_mob_soft.Domain.Models.Db;   // сущности БД
using kat_mob_soft.Domain.ViewModels; // DTO/ViewModels

namespace kat_mob_soft.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // Маппинг UserDb <-> ProfileViewModel
            CreateMap<UserDb, ProfileViewModel>();

            // Маппинг RegisterViewModel -> UserDb
            CreateMap<RegisterViewModel, UserDb>();

            // Маппинг UpdateProfileViewModel -> UserDb
            CreateMap<UpdateProfileViewModel, UserDb>();

            // Другие маппинги по необходимости
        }
    }
}
