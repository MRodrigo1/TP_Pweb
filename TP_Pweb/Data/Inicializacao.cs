﻿using Microsoft.AspNetCore.Identity;
using System;
using TP_Pweb.Models;

namespace PWEB_AulasP_2223.Data
{
    public enum Roles
    {
        Cliente,
        Funcionario,
        Gestor,
        Administrador
    }
    public static class Inicializacao
    {
        public static async Task CriaDadosIniciais(UserManager<Utilizador>
       userManager, RoleManager<IdentityRole> roleManager)
        {
            //Adicionar default Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Cliente.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Funcionario.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Gestor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));
            //Adicionar Default User - Utilizador Anónimo
            var defaultUser = new Utilizador
            {
                UserName = "admin@localhost.com",
                Email = "admin@localhost.com",
                PrimeiroNome = "Administrador",
                UltimoNome = "Local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Is3C..00");
                await userManager.AddToRoleAsync(defaultUser,
                Roles.Administrador.ToString());
            }
        }
    }
}
