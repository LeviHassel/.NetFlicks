﻿using CoreTemplate.Accessors.Models.DTO.Base;

namespace CoreTemplate.Accessors.Models.DTO
{
    public class MovieGenreDTO : EntityDTO
    {
        public int MovieId { get; set; }

        public MovieDTO Movie { get; set; }

        public int GenreId { get; set; }

        public GenreDTO Genre { get; set; }
    }
}
