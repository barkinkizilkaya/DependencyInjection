using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjectionApp.AppSettingClasses;
using InjectionApp.Models;
using InjectionApp.Servis;
using InjectionApp.Servis.LifeTimeExample;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InjectionApp.Pages
{
    public class IndexModel : PageModel
    {
        public string MarketAciklamasi { get; set; }
        public string ConnectionAdress { get; set; }

        private readonly ILogger _logger;
        private readonly IEkonomiTahmini _ekonomiTahmini;
        private readonly ConnectionSettings _connectionOptions;

        public List<string> LifeTime { get; set; }
        private readonly TransientService _transientService;
        private readonly ScopedService _scopedService;
        private readonly SingletonService _singletonService;

        public IndexModel(ILogger<IndexModel> logger,IEkonomiTahmini ekonomiTahmini,IOptions<ConnectionSettings> connectionOptions, TransientService transientService, ScopedService scopedService, SingletonService singletonService)
        {
            _ekonomiTahmini = ekonomiTahmini;
            _connectionOptions = connectionOptions.Value;
            _scopedService = scopedService;
            _singletonService = singletonService;
            _transientService = transientService;
            _logger = logger;

        }

        public void OnGet()
        {
            _logger.LogInformation("Web uygulaması açıldı! koşun");

            LifeTime = new List<string>()
            {
                "Transient : "+_transientService.GetGuid()+" ",
                "Scoped : "+_scopedService.GetGuid()+"",
                "SingleTon : "+_singletonService.GetGuid()+""
            };

            ConnectionAdress = _connectionOptions.DefaultConnection;
            MarketSonuc marketSonuc = _ekonomiTahmini.marketSonucTahmin();

            switch (marketSonuc.eknomikDurum)
            {
                case EkonomiDurumu.iyi:
                    MarketAciklamasi = "Durumlar fena değil";
                    break;
                case EkonomiDurumu.Kotu:
                    MarketAciklamasi = "Durumlar fena";
                    break;
                case EkonomiDurumu.Cirkin:
                    MarketAciklamasi = "Durumlar ne olacak belli değil";
                    break;


            }
        }
    }
}
