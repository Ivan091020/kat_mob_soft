using AutoMapper;
using kat_mob_soft.Domain.Models.Db;   // сущности БД
using kat_mob_soft.Domain.ViewModels; // DTO/ViewModels

namespace kat_mob_soft.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // Маппинг UserDb -> ProfileViewModel
            CreateMap<UserDb, ProfileViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.DisplayName ?? src.Username));

            // Маппинг RegisterViewModel -> UserDb (Password игнорируется, т.к. будет хешироваться отдельно)
            CreateMap<RegisterViewModel, UserDb>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.AvatarPath, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.Downloads, opt => opt.Ignore())
                .ForMember(dest => dest.Purchases, opt => opt.Ignore())
                .ForMember(dest => dest.ReportsFiled, opt => opt.Ignore())
                .ForMember(dest => dest.AuditLogs, opt => opt.Ignore());

            // Маппинг UpdateProfileViewModel -> UserDb (только для обновления)
            CreateMap<UpdateProfileViewModel, UserDb>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Username, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.AvatarPath, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.Downloads, opt => opt.Ignore())
                .ForMember(dest => dest.Purchases, opt => opt.Ignore())
                .ForMember(dest => dest.ReportsFiled, opt => opt.Ignore())
                .ForMember(dest => dest.AuditLogs, opt => opt.Ignore())
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FullName ?? src.DisplayName));

            // Другие маппинги по необходимости
        }
    }
}
