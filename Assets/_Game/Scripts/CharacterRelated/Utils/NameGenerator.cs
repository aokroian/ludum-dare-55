
using UnityEngine;

namespace Utils
{
    public class NameGenerator
    {
        // array of different male first names
        private static readonly string[] MaleFirstNames =
        {
            "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles",
            "Joseph", "Thomas",
            "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven",
            "Edward", "Brian",
            "Ronald", "Anthony", "Kevin", "Jason", "Matthew", "Gary", "Timothy", "Jose", "Larry",
            "Jeffrey", "Frank",
            "Scott", "Eric", "Stephen", "Andrew", "Raymond", "Gregory", "Joshua", "Jerry", "Dennis",
            "Walter",
            "Patrick", "Peter", "Harold", "Douglas", "Henry", "Carl", "Arthur", "Ryan", "Roger",
            "Joe", "Juan",
            "Jack", "Albert", "Jonathan", "Justin", "Terry", "Gerald", "Keith", "Samuel", "Willie",
            "Ralph", "Lawrence",
            "Nicholas", "Roy", "Benjamin", "Bruce", "Brandon", "Adam", "Harry", "Fred", "Wayne",
            "Billy", "Steve",
            "Louis", "Jeremy", "Aaron", "Randy", "Howard", "Eugene", "Carlos", "Russell", "Bobby",
            "Victor", "Martin",
            "Ernest", "Phillip", "Todd", "Jesse", "Craig", "Alan", "Shawn", "Clarence", "Sean",
            "Philip", "Chris",
            "Johnny", "Earl", "Jimmy", "Antonio", "Danny", "Bryan", "Tony", "Luis", "Mike",
            "Stanley", "Leonard",
            "Nathan", "Dale", "Manuel", "Rodney", "Curtis", "Norman", "Allen", "Marvin", "Vincent",
            "Glenn", "Jeffery",
            "Travis", "Jeff", "Chad", "Jacob", "Lee", "Melvin", "Alfred", "Kyle", "Francis",
            "Bradley", "Jesus",
            "Herbert", "Frederick", "Ray", "Joel", "Edwin", "Don", "Eddie", "Ricky"
        };

        public static string GetRandomName()
        {
            return MaleFirstNames[Random.Range(0, MaleFirstNames.Length)];
        }
    }
}