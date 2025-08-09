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
		public static List<Product> ShuffleGearShop(bool enable, MT19337 rng)
		{
			if (!enable)
			{
				return new();
			}

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
					armors.Add(product.value);
				}
				else if(weaponShops.ContainsKey((Shops)product.Value.GroupId))
				{
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
					product.GroupId = (int)shop.Key;
					newShops.Add(product);
				}
			}

			foreach (var shop in armorShops)
			{
				foreach (var product in shop.Value)
				{
					product.GroupId = (int)shop.Key;
					newShops.Add(product);
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
		public static void LoadShuffledShops(List<Product> shops)
		{
			foreach (var product in shops)
			{
				FF1PR.MasterManager.GetList<Product>()[product.Id] = product;
			}
		}
	}
}
