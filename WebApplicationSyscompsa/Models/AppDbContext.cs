using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationSyscompsa.Models.APCIAS_MODEL;
using WebApplicationSyscompsa.Models.ar_loscann;
using WebApplicationSyscompsa.Models.code_color_lab;
using WebApplicationSyscompsa.Models.cosecha_control;
using WebApplicationSyscompsa.Models.despacho_save;
using WebApplicationSyscompsa.Models.history;
using WebApplicationSyscompsa.Models.lote_foto;
using WebApplicationSyscompsa.Models.Mail_Send;
using WebApplicationSyscompsa.Models.Mod_tab;
using WebApplicationSyscompsa.Models.traspase_product;

namespace WebApplicationSyscompsa.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
        public DbSet<WebUser> WebUser { get; set; }
        public DbSet<apcias> apcias { get; set; }
        public DbSet<mensajeMod> mensajeMod { get; set; }
        public DbSet<t_invcabg> t_invcabg { get; set; }
        public DbSet<t_invdetg> t_invdetg { get; set; }
        public DbSet<Img_lote> img_lote { get; set; }
        public DbSet<Tipo_control_emps> tipo_control_emp { get; set; }
        public DbSet<Traspase_product> traspase_product { get; set; }
        public DbSet<Module_tab> module_tab { get; set; }
        public DbSet<Ar_loscann> ar_loscann { get; set; }
        public DbSet<Hist_comp_emp> hist_comp_emp { get; set; }
        public DbSet<AlptablaModel> alptabla { get; set; }
        public DbSet<Code_color_lab> code_color_lab { get; set; }
        public DbSet<Dp08acal> dp08acal { get; set; }
        public DbSet<Audit_print_lote> audit_print_lote { get; set; }
        public DbSet<C_DEVSOB> C_DEVSOB { get; set; }
        public DbSet<Control_cosecha> control_cosecha { get; set; }
                       
                       
                       
        //asignamos e  l valor de los decimales que estan truncandose mediante c#
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // CAMBIAMOS EL PRIMARY KEY QUE VIEN POR DEFECTO EN ID Y ESCIFICAMOS EL QUE QUEREMOS USAR
            #region
            //t_invcabg
            modelBuilder.Entity<apcias>().HasKey(pk => pk.email_despacho_web).HasName("email_despacho_web");
            modelBuilder.Entity<t_invcabg>().HasKey(pk => pk.T_llave).HasName("T_llave");
            modelBuilder.Entity<t_invcabg>().HasKey(pk => pk.tempo).HasName("tempo");
            modelBuilder.Entity<Img_lote>().HasKey(pk =>  pk.no_parte_i).HasName("no_parte_i");
            modelBuilder.Entity<Code_color_lab>().HasKey(pk =>  pk.hex_cod_color).HasName("hex_cod_color");
            // modelBuilder.Entity<Code_color_lab>().HasKey(pk =>  pk.name_labor).HasName("name_labor");
            // modelBuilder.Entity<AlptablaModel>().HasNoKey();
            #endregion

            //EVITAMOS EL CRASHED(COFLICTO) DE DATOS
            #region
            //t_invdetg

            modelBuilder.Entity<C_DEVSOB>().HasKey(pk => pk.lote_prod).HasName("lote_prod");
            modelBuilder.Entity<t_invdetg>().HasKey(pk => pk.T_llave).HasName("T_llave");
            modelBuilder.Entity<t_invdetg>().HasKey(pk => pk.tempo).HasName("tempo");
            modelBuilder.Entity<Module_tab>().HasKey(pk => pk.name_module).HasName("name_module");
            modelBuilder.Entity<t_invdetg>().HasKey(pk => pk.linea).HasName("linea");
            modelBuilder.Entity<Dp08acal>().HasKey(pk => pk.anio).HasName("anio");
            modelBuilder.Entity<Dp08acal>().HasKey(pk => pk.peri).HasName("peri");
            modelBuilder.Entity<Dp08acal>().HasKey(pk => pk.sema).HasName("sema");
            modelBuilder.Entity<t_invdetg>().Property(a => a.cantidad).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<t_invdetg>().Property(a => a.precio_t).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<t_invdetg>().Property(a => a.precio_u).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<t_invdetg>().Property(a => a.cant_tou).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<t_invcabg>().Property(a => a.total_mov).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<t_invcabg>().Property(a => a.total_trn).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Hist_comp_emp>().Property(a => a.cant_desp).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Hist_comp_emp>().Property(a => a.cant_total).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<C_DEVSOB>().Property(a => a.cant).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<C_DEVSOB>().Property(a => a.cant_dev).HasColumnType("decimal(10,2)");
            //alptabla
            modelBuilder.Entity<Dp08acal>().Property(a => a.ncaja).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<AlptablaModel>().Property(a => a.valor).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<AlptablaModel>().Property(a => a.lencod).HasColumnType("decimal(2,0)");

            modelBuilder.Entity<Traspase_product>().Property(a => a.cantidad).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Traspase_product>().Property(a => a.difer_stcok).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Traspase_product>().Property(a => a.stock).HasColumnType("decimal(10,2)");

            #endregion

        }

    }
}

