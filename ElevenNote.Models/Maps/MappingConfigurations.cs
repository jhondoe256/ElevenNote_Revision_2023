using AutoMapper;
using ElevenNote.Data;
using ElevenNote.Models.Category;
using ElevenNote.Models.Note;

namespace ElevenNote.Models.Maps
{
    public class MappingConfigurations : Profile
    {
        public MappingConfigurations()
        {
            CreateMap<NoteEntity, NoteDetail>().ReverseMap();
            CreateMap<NoteEntity, NoteListItem>().ReverseMap();

            CreateMap<NoteCreate, NoteEntity>()
                    .ForMember(note => note.CreatedUtc, opt => opt
                    .MapFrom(src => DateTimeOffset.Now));

            CreateMap<NoteEdit, NoteEntity>()
                    .ForMember(note => note.ModifiedUtc, opt => opt
                    .MapFrom(src => DateTimeOffset.Now));

            CreateMap<CategoryEntity, CategoryListItem>().ReverseMap();
            CreateMap<CategoryEntity, CategoryDetail>().ReverseMap();
            CreateMap<CategoryEntity, CategoryCreate>().ReverseMap();
            CreateMap<CategoryEntity, CategoryEdit>().ReverseMap();
        }
    }
}