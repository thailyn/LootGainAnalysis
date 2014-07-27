using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib
{
    public class DecisionTreeNode
    {
        private DataSourcesCollection _sources;
        public DataSourcesCollection Sources
        {
            get
            {
                return _sources;
            }
            set
            {
                _sources = value;
                RefreshSourcesInformation();
            }
        }

        private int _itemId;
        public int ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = value;
                RefreshSourcesInformation();
            }
        }

        public double Entropy { get; set; }
        public double Probability { get; set; }
        public DataSourcesCollection Positive { get; set; }
        public DataSourcesCollection Negative { get; set; }

        public Attribute ParentSplitAttribute { get; set; }
        public object ParentSplitAttributeValue { get; set; }

        public Attribute SplitAttribute { get; set; }
        public object SplitAttributeValue { get; set; }
        public double SplitAttributeInformationGain { get; set; }

        public Dictionary<object, DecisionTreeNode> Children { get; set; }

        public DecisionTreeNode()
        {
            Children = new Dictionary<object, DecisionTreeNode>();
        }

        protected void RefreshSourcesInformation()
        {
            if (Sources != null && Sources.Count > 0)
            {
                Entropy = Sources.EntropyOnItemId(ItemId);

                DataSourcesCollection positive, negative;
                Sources.DivideOnHavingItem(ItemId, out positive, out negative);
                Probability = (double) positive.Count / Sources.Count;
                Positive = positive;
                Negative = negative;
            }
            else
            {
                Entropy = double.NaN;
                Probability = double.NaN;
                Positive = null;
                Negative = null;
            }
        }

        public void CreateChildrenOnItemId(int itemId, AttributeValues attributeValues)
        {
            if (double.IsNaN(Entropy) || Entropy < 0.0001)
            {
                //System.Console.WriteLine("Will not create children.  Entropy of {0} is below threshold.", Entropy);
                return;
            }

            Attribute bestAttribute;
            object bestAttributeValue;

            SplitAttributeInformationGain = Sources.FindGreatestInformationGain(itemId, attributeValues,
                out bestAttribute, out bestAttributeValue);
            SplitAttribute = bestAttribute;
            SplitAttributeValue = bestAttributeValue;

            //if (SplitAttributeInformationGain < Entropy / 100)
            if (SplitAttributeInformationGain < Entropy / 10000000)
            {
                //System.Console.WriteLine("Will not create children.  Information gain of {0} is below threshold.",
                //    SplitAttributeInformationGain);
                return;
            }
                        
            Dictionary<object, DataSourcesCollection> splits;
            Sources.DivideOnAttribute(bestAttribute, bestAttributeValue, attributeValues.ValuesMap[bestAttribute],
                out splits);

            foreach (var split in splits)
            {
                split.Value.UseAttributesWithValues = Sources.UseAttributesWithValues;

                DecisionTreeNode child = new DecisionTreeNode
                {
                    Sources = split.Value,
                    ItemId = itemId,
                    ParentSplitAttribute = SplitAttribute,
                    ParentSplitAttributeValue = split.Key,
                };
                                
                Children.Add(split.Key, child);
                child.CreateChildrenOnItemId(itemId, attributeValues);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (ParentSplitAttributeValue != null)
            {
                builder.Append(string.Format("{0}: {1}", ParentSplitAttribute.ToString(), ParentSplitAttributeValue.ToString()));
            }
            else
            {
                builder.Append("N/A");
            }

            builder.Append(string.Format(", {0}% ({1}/{2}) / {3}", Probability * 100.0, Positive.Count, Sources.Count,
                Entropy));

            builder.Append(" -> ");
            if (SplitAttributeInformationGain > 0)
            {
                builder.Append(SplitAttribute.ToString());
            }
            else
            {
                builder.Append("N/A");
            }

            return builder.ToString();
        }

        public void ConsolePrint(string indent)
        {
            System.Console.WriteLine(indent + ToString());
            foreach (var child in Children)
            {
                if (child.Value.Probability > 0)
                {
                    child.Value.ConsolePrint(indent + "\t");
                }
            }
        }
    }
}
