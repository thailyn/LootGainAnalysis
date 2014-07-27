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
    }
}
