using sports_game.src.Handlers;

namespace sports_game.src.Services
{
    public class CategoryService(int seed)
    {
        public List<Category> Categories = [];
        private readonly Random _random = new(seed);

        public void AddCategory(Category category)
        {
            Categories.Add(category);
        }

        public void DistributeItems(List<Person> people)
        {
            foreach (var person in people)
            {
                var category = Categories.FirstOrDefault(c => c.Name == person.Rarity);
                category?.People.Add(person);
            }
        }

        public void RemoveItem(Person person)
        {
            var category = Categories.FirstOrDefault(c => c.Name == person.Rarity);
            category?.People.Remove(person);
        }

        public void AddItem(Person person)
        {
            var category = Categories.FirstOrDefault(c => c.Name == person.Rarity);
            category?.People.Add(person);
        }

        public Person PickItem()
        {
            double totalWeight = Categories.Sum(c => c.Weight);
            double randomValue = _random.NextDouble() * totalWeight;

            while (true){
                foreach (var category in Categories)
                {
                    if (randomValue < category.Weight)
                    {
                        if (category.People.Count != 0)
                        {
                            int itemIndex = _random.Next(category.People.Count);
                            return category.People[itemIndex];
                        }
                    }

                    randomValue -= category.Weight;
                }
            }
        }
    }
}