﻿namespace MyBitly.DAL
{
    using System.Data.Entity;
    using Entities;

    public class MyBitlyContext : DbContext
    {
        public MyBitlyContext() : base("Server=OMA-PC;Database=mybitly;User Id=sa;Password=7777777;")
        {
        }

        public virtual DbSet<UrlEntity> Urls { get; set; } 
    }
}