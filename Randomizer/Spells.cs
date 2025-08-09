using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last.Data.Master;
using UnityEngine.Bindings;
using Mono.Cecil;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public class ShuffledSpell
		{
			public int ProductId { get; set; }
			public int ContentId { get; set; }
			public int TargetId { get; set; }
			public int Permission { get; set; }
			public int Cost { get; set; }
			public int Level { get; set; }
		}

		public static List<ShuffledSpell> ShuffleSpells(bool enable, MT19337 rng)
		{
			if (!enable)
			{
				return new();
			}

			Dictionary<Shops, List<Product>> whiteShops = new()
			{
				{ Shops.ConeriaWhiteMagicShop, new() },
				{ Shops.PravokaWhiteMagicShop, new() },
				{ Shops.ElflandWhiteMagicShop1, new() },
				{ Shops.ElflandWhiteMagicShop2, new() },
				{ Shops.MelmondWhiteMagicShop, new() },
				{ Shops.CrescentWhiteMagicShop, new() },
				{ Shops.OnracWhiteMagicShop, new() },
				{ Shops.GaiaWhiteMagicShop1, new() },
				{ Shops.GaiaWhiteMagicShop2, new() },
				{ Shops.LufeniaWhiteMagicShop, new() },
			};

			Dictionary<Shops, List<Product>> blackShops = new()
			{
				{ Shops.ConeriaBlackMagicShop, new() },
				{ Shops.PravokaBlackMagicShop, new() },
				{ Shops.ElflandBlackMagicShop1, new() },
				{ Shops.ElflandBlackMagicShop2, new() },
				{ Shops.MelmondBlackMagicShop, new() },
				{ Shops.CrescentBlackMagicShop, new() },
				{ Shops.OnracBlackMagicShop, new() },
				{ Shops.GaiaBlackMagicShop1, new() },
				{ Shops.GaiaBlackMagicShop2, new() },
				{ Shops.LufeniaBlackMagicShop, new() },
			};

			List<Ability> whiteSpells = new();
			List<Ability> blackSpells = new();
			List<Product> whiteProducts = new();
			List<Product> blackProducts = new();
			Dictionary<int, int> contentToSpellId = new();
			Dictionary<int, int> spellToContentId = new();

			foreach (var ability in FF1PR.MasterManager.GetList<Ability>())
			{
				if (ability.Value.TypeId == 1 && ability.Value.Id <= 68)
				{
					whiteSpells.Add(ability.value);
				}
				else if(ability.Value.TypeId == 2 && ability.Value.Id <= 68)
				{
					blackSpells.Add(ability.Value);
				}
			}

			foreach (var product in FF1PR.MasterManager.GetList<Product>())
			{
				if (whiteShops.ContainsKey((Shops)product.value.GroupId))
				{
					whiteProducts.Add(product.value);
					var spellId = FF1PR.MasterManager.GetData<Content>(product.value.ContentId).TypeValue;
					contentToSpellId.Add(product.value.ContentId, spellId);
					spellToContentId.Add(spellId, product.value.ContentId);
				}
				else if (blackShops.ContainsKey((Shops)product.value.GroupId))
				{
					blackProducts.Add(product.value);
					var spellId = FF1PR.MasterManager.GetData<Content>(product.value.ContentId).TypeValue;
					contentToSpellId.Add(product.value.ContentId, spellId);
					spellToContentId.Add(spellId, product.value.ContentId);
				}
			}

			List<ShuffledSpell> shuffledSpells = new();

			List<int> whiteSpellIds = whiteSpells.Select(s => s.Id).ToList();
			foreach (var product in whiteProducts)
			{
				var spelldata = whiteSpells.Find(s => s.Id == contentToSpellId[product.ContentId]);
				var targetspell = rng.TakeFrom(whiteSpellIds);

				shuffledSpells.Add(new ShuffledSpell() { ProductId = product.Id, ContentId = spellToContentId[targetspell], TargetId = targetspell, Cost = spelldata.Buy, Level = spelldata.AbilityLv, Permission = spelldata.UseJobGroupId });
			}

			List<int> blackSpellIds = blackSpells.Select(s => s.Id).ToList();
			foreach (var product in blackProducts)
			{
				var spelldata = blackSpells.Find(s => s.Id == contentToSpellId[product.ContentId]);
				var targetspell = rng.TakeFrom(blackSpellIds);

				shuffledSpells.Add(new ShuffledSpell() { ProductId = product.Id, ContentId = spellToContentId[targetspell], TargetId = targetspell, Cost = spelldata.Buy, Level = spelldata.AbilityLv, Permission = spelldata.UseJobGroupId });
			}

			return shuffledSpells;
		}

		public static void LoadShuffledSpells(List<ShuffledSpell> shuffledSpells)
		{
			foreach (var spell in shuffledSpells)
			{
				FF1PR.MasterManager.GetData<Product>(spell.ProductId).ContentId = spell.ContentId;
				var targetspell = FF1PR.MasterManager.GetData<Ability>(spell.TargetId);
				targetspell.AbilityLv = spell.Level;
				targetspell.Buy = spell.Cost;
				targetspell.UseJobGroupId = spell.Permission;
			}
		}
	}
}
