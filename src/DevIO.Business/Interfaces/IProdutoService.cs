﻿using DevIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProdutoService
    {
        Task Add(Produto produto);

        Task Update(Produto produto);

        Task Delete(Guid id);
    }
}