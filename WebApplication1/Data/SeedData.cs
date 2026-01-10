using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            // Tworzenie ról
            string[] roleNames = { "Administrator", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Tworzenie administratora
            var adminEmail = "admin@vstvault.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Administrator");
                }
            }

            if (!context.Plugins.Any())
            {
                var plugins = new List<Plugin>
                {
                    // FabFilter wtyczki
                    new Plugin
                    {
                        Name = "FabFilter Pro-Q 3",
                        Version = "VST3",
                        Price = 169.00m,
                        ManufacturerId = 1, // FabFilter
                        CategoryId = 4, // EQ
                        Description = "Profesjonalny korektor parametryczny z funkcją analizy spektrum i dynamicznego EQ. Idealny do masteringu i miksu.",
                        SystemRequirements = "Windows 10/11, macOS 10.13+, 4GB RAM, VST3/AU/AAX"
                    },
                    new Plugin
                    {
                        Name = "FabFilter Pro-C 2",
                        Version = "VST3",
                        Price = 139.00m,
                        ManufacturerId = 1,
                        CategoryId = 2, // Kompresor
                        Description = "Wszechstronny kompresor z 8 trybami działania, od vintage do nowoczesnego. Precyzyjna kontrola dynamiki.",
                        SystemRequirements = "Windows 10/11, macOS 10.13+, 4GB RAM, VST3/AU/AAX"
                    },
                    new Plugin
                    {
                        Name = "FabFilter Pro-R",
                        Version = "VST3",
                        Price = 139.00m,
                        ManufacturerId = 1,
                        CategoryId = 3, // Reverb
                        Description = "Wysokiej jakości pogłos algorytmiczny z intuicyjnym interfejsem. Idealny do wokali i instrumentów.",
                        SystemRequirements = "Windows 10/11, macOS 10.13+, 4GB RAM, VST3/AU/AAX"
                    },

                    // Xfer Records
                    new Plugin
                    {
                        Name = "Serum",
                        Version = "VST3",
                        Price = 189.00m,
                        ManufacturerId = 2, // Xfer Records
                        CategoryId = 1, // Syntezator
                        Description = "Zaawansowany syntezator wavetable z oszałamiającą jakością dźwięku. Ulubiony wybór producentów EDM.",
                        SystemRequirements = "Windows 8+, macOS 10.8+, 8GB RAM, VST3/AU/AAX"
                    },
                    new Plugin
                    {
                        Name = "Serum FX",
                        Version = "VST3",
                        Price = 89.00m,
                        ManufacturerId = 2,
                        CategoryId = 2, // Kompresor
                        Description = "Sekcja efektów z Serum jako osobna wtyczka. 10 efektów wysokiej jakości.",
                        SystemRequirements = "Windows 8+, macOS 10.8+, 4GB RAM, VST3/AU/AAX"
                    },

                    // Native Instruments
                    new Plugin
                    {
                        Name = "Massive X",
                        Version = "VST3",
                        Price = 199.00m,
                        ManufacturerId = 3, // Native Instruments
                        CategoryId = 1, // Syntezator
                        Description = "Następca legendarnego Massive. Potężny syntezator z nowoczesną architekturą dźwięku.",
                        SystemRequirements = "Windows 10/11, macOS 10.14+, 8GB RAM, VST3/AU/AAX"
                    },
                    new Plugin
                    {
                        Name = "Kontakt 7",
                        Version = "VST3",
                        Price = 399.00m,
                        ManufacturerId = 3,
                        CategoryId = 1, // Syntezator
                        Description = "Branżowy standard samplera i sampler-based synthesizer. Ponad 55GB biblioteki dźwięków.",
                        SystemRequirements = "Windows 10/11, macOS 10.14+, 16GB RAM, 70GB HDD, VST3/AU/AAX"
                    },
                    new Plugin
                    {
                        Name = "Raum",
                        Version = "VST3",
                        Price = 99.00m,
                        ManufacturerId = 3,
                        CategoryId = 3, // Reverb
                        Description = "Kreatywny pogłos z unikalnym charakterem. Od subtelnych przestrzeni po ekstremalne efekty.",
                        SystemRequirements = "Windows 10/11, macOS 10.14+, 4GB RAM, VST3/AU/AAX"
                    },
                    new Plugin
                    {
                        Name = "Solid Bus Comp",
                        Version = "VST3",
                        Price = 149.00m,
                        ManufacturerId = 3,
                        CategoryId = 2, // Kompresor
                        Description = "Emulacja klasycznego kompresora szynowego. Idealny do kleju miksu i masteringu.",
                        SystemRequirements = "Windows 10/11, macOS 10.14+, 4GB RAM, VST3/AU/AAX"
                    },

                    new Plugin
                    {
                        Name = "FabFilter Saturn 2",
                        Version = "VST3",
                        Price = 139.00m,
                        ManufacturerId = 1,
                        CategoryId = 2, // Kompresor
                        Description = "Multiband distortion i saturation. Od subtelnego ciepła po agresywne zniekształcenia.",
                        SystemRequirements = "Windows 10/11, macOS 10.13+, 4GB RAM, VST3/AU/AAX"
                    },
                    new Plugin
                    {
                        Name = "FabFilter Timeless 3",
                        Version = "VST3",
                        Price = 99.00m,
                        ManufacturerId = 1,
                        CategoryId = 2, // Kompresor
                        Description = "Kreatywny delay z filtracją i modulacją. Od prostych opóźnień po złożone przestrzenne efekty.",
                        SystemRequirements = "Windows 10/11, macOS 10.13+, 4GB RAM, VST3/AU/AAX"
                    },
                    new Plugin
                    {
                        Name = "Native Instruments Replika",
                        Version = "VST3",
                        Price = 99.00m,
                        ManufacturerId = 3,
                        CategoryId = 2, // Kompresor
                        Description = "Trzy klasyczne urządzenia delay w jednej wtyczce. Od vintage po nowoczesny dźwięk.",
                        SystemRequirements = "Windows 10/11, macOS 10.14+, 4GB RAM, VST3/AU/AAX"
                    }
                };

                context.Plugins.AddRange(plugins);
                await context.SaveChangesAsync();
            }
        }
    }
}
