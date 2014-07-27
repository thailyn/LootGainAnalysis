using LootGainLib.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib
{
    public class DataSourcesCollection : ObservableCollection<DataSource>
    {
        public void DivideOnAttribute(Attribute attribute, object attributeValue,
            Dictionary<object, int> attributeValues, out Dictionary<object, DataSourcesCollection> splits)
        {
            splits = new Dictionary<object, DataSourcesCollection>();
            switch (attribute)
            {
                case Attribute.Quest:
                    splits.Add("pos", new DataSourcesCollection());
                    splits.Add("neg", new DataSourcesCollection());
                    break;
                default:
                    foreach (var value in attributeValues.Keys)
                    {
                        splits.Add(value, new DataSourcesCollection());
                    }
                    break;
            }

            foreach (var source in this)
            {
                switch (attribute)
                {
                    case Attribute.Build:
                        splits[source.Build].Add(source);
                        break;
                    case Attribute.PlayerName:
                        splits[source.PlayerName].Add(source);
                        break;
                    case Attribute.PlayerRace:
                        splits[source.PlayerRace].Add(source);
                        break;
                    case Attribute.PlayerClass:
                        splits[source.PlayerClass].Add(source);
                        break;
                    case Attribute.ZoneName:
                        splits[source.ZoneName].Add(source);
                        break;
                    case Attribute.SubZoneName:
                        splits[source.SubZoneName].Add(source);
                        break;
                    case Attribute.Quest:
                        if (source.HasQuest((int)attributeValue))
                        {
                            splits["pos"].Add(source);
                        }
                        else
                        {
                            splits["neg"].Add(source);
                        }
                        break;
                    case Attribute.SourceName:
                        splits[source.SourceName].Add(source);
                        break;
                    case Attribute.LootType:
                        splits[source.LootType].Add(source);
                        break;
                    case Attribute.Loot:
                        // do something?
                        break;
                }
            }
        }

        public void DivideOnHavingItem(int itemId, out DataSourcesCollection positive,
            out DataSourcesCollection negative)
        {
            positive = new DataSourcesCollection();
            negative = new DataSourcesCollection();

            foreach (var source in this)
            {
                if (source.HasLoot(itemId))
                {
                    positive.Add(source);
                }
                else
                {
                    negative.Add(source);
                }
            }
        }

        public double EntropyOnItemId(int itemId)
        {
            if (this.Count == 0)
            {
                return 0;
            }

            List<DataSourcesCollection> splits = new List<DataSourcesCollection>(2);
            DataSourcesCollection positive, negative;

            DivideOnHavingItem(itemId, out positive, out negative);
            splits.Add(positive);
            splits.Add(negative);

            double sum = 0;
            foreach(var split in splits)
            {
                if (split.Count > 0)
                {
                    double probability = (double) split.Count / this.Count;
                    sum += probability * System.Math.Log(probability, 2);
                }
            }

            return -1 * sum;
        }

        public double InformationGainOnItemId(int itemId, Attribute attribute, object attributeValue,
            Dictionary<object, int> attributeValues)
        {
            Dictionary<object, DataSourcesCollection> splits;
            double baseEntropy = EntropyOnItemId(itemId);
            DivideOnAttribute(attribute, attributeValue, attributeValues, out splits);

            double sum = 0;
            foreach (var split in splits.Keys)
            {
                double splitEntropy = splits[split].EntropyOnItemId(itemId);
                sum += splitEntropy * ((double) splits[split].Count / this.Count);
            }

            return baseEntropy - sum;
        }
    }
}
