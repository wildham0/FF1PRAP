using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last.Data.Master;
using UnityEngine.Bindings;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static List<Product> ShuffleGearShop(bool enable, uint seed)
		{
			if (!enable)
			{
				return new();
			}

			MT19337 rng = new MT19337(seed);

			Dictionary<Shops, List<Product>> weaponShops = new()
			{
				{ Shops.ConeriaWeaponShop, new() },
				{ Shops.PravokaWeaponShop, new() },
				{ Shops.ElflandWeaponShop, new() },
				{ Shops.MelmondWeaponShop, new() },
				{ Shops.CrescentWeaponShop, new() },
				{ Shops.GaiaWeaponShop, new() },
			};

			Dictionary<Shops, List<Product>> armorShops = new()
			{
				{ Shops.ConeriaArmorShop, new() },
				{ Shops.PravokaArmorShop, new() },
				{ Shops.ElflandArmorShop, new() },
				{ Shops.MelmondArmorShop, new() },
				{ Shops.CrescentArmorShop, new() },
				{ Shops.GaiaArmorShop, new() },
			};

			List<Product> armors = new();
			List<Product> weapons = new();

			foreach (var product in FF1PR.MasterManager.GetList<Product>())
			{
				if (armorShops.ContainsKey((Shops)product.Value.GroupId))
				{
					//product.Value.GroupId = 255;
					armors.Add(product.value);
				}
				else if(weaponShops.ContainsKey((Shops)product.Value.GroupId))
				{
					//product.Value.GroupId = 255;
					weapons.Add(product.value);
				}
			}

			ShuffleShop(ref weaponShops, weapons, rng);
			ShuffleShop(ref armorShops, armors, rng);

			List<Product> newShops = new();
			foreach (var shop in weaponShops)
			{
				foreach (var product in shop.Value)
				{
					//var productToAdd = new Product();
					product.GroupId = (int)shop.Key;

					/*
					productToAdd.Id = keyBase++;
					productToAdd.ContentId = product;
					productToAdd.GroupId = (int)shop.Key;
					productToAdd.Coefficient = 0;
					productToAdd.PurchaseLimit = 99;*/
					newShops.Add(product);
					//FF1PR.MasterManager.GetList<Product>().Add(productToAdd.Id, productToAdd);
				}
			}

			foreach (var shop in armorShops)
			{
				foreach (var product in shop.Value)
				{
					product.GroupId = (int)shop.Key;
					//var productToAdd = new Product();
					/*
					productToAdd.Id = keyBase++;
					productToAdd.ContentId = product;
					productToAdd.GroupId = (int)shop.Key;
					productToAdd.Coefficient = 0;
					productToAdd.PurchaseLimit = 99;*/
					newShops.Add(product);
					//FF1PR.MasterManager.GetList<Product>().Add(productToAdd.Id, productToAdd);
				}
			}

			return newShops;
		}

		private static void ShuffleShop(ref Dictionary<Shops, List<Product>> shops, List<Product> items, MT19337 rng)
		{
			while (items.Any())
			{
				Product itemToPlace = rng.TakeFrom(items);

				List<Shops> validShops;
				validShops = shops.Where(s => s.Value.Count <= 0).Select(s => s.Key).ToList();

				if (!validShops.Any())
				{
					validShops = shops.Where(
						s => !s.Value.Where(p => p.ContentId == itemToPlace.ContentId).Any() &&
						(s.Value.Count < 5 || rng.Between(1, 3) == 3)
						).Select(s => s.Key).ToList();
				}

				if (!validShops.Any())
				{
					validShops = shops.Where(s => !s.Value.Where(p => p.ContentId == itemToPlace.ContentId).Any()).Select(s => s.Key).ToList();
				}

				if (!validShops.Any())
				{
					throw new Exception("Shop Shuffle - Impossible Item Source");
				}

				var shop = rng.PickFrom(validShops);

				shops[shop].Add(itemToPlace);
			}
		}
    }
}
