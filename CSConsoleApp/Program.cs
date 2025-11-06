using System;
using System.Collections.Generic;
using System.Linq;

namespace CSConsoleApp
{
    public static class Program
    {
        public static void Main()
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var filePath = System.IO.Directory.GetFiles(currentDirectory, "*.csv").First();

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("CSV файл не найден в текущей директории.");
                return;
            }

            IReadOnlyList<MovieCredit> movieCredits;
            try
            {
                var parser = new MovieCreditsParser(filePath);
                movieCredits = parser.Parse();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Не удалось распарсить csv: {exc.Message}");
                return;
            }

            ExecuteAllTasks(movieCredits);
        }

        public static void ExecuteAllTasks(IReadOnlyList<MovieCredit> movieCredits)
        {
            Console.WriteLine("Задание 1: Фильмы Стивена Спилберга");
            var spielbergMovies = GetMoviesByDirector(movieCredits, "Steven Spielberg");
            Console.WriteLine(string.Join(Environment.NewLine, spielbergMovies));

            Console.WriteLine("\nЗадание 2: Персонажи Тома Хэнкса");
            var tomHanksCharacters = GetCharactersByActor(movieCredits, "Tom Hanks");
            Console.WriteLine(string.Join(Environment.NewLine, tomHanksCharacters));

            Console.WriteLine("\nЗадание 3: Топ-5 фильмов по количеству актеров");
            var top5MoviesByCastSize = GetTopMoviesByCastSize(movieCredits, 5);
            Console.WriteLine(string.Join(Environment.NewLine, top5MoviesByCastSize.Select(m => $"{m.Title} - {m.Cast.Count} актеров")));

            Console.WriteLine("\nЗадание 4: Топ-10 востребованных актеров");
            var top10Actors = GetTopActorsByMovieCount(movieCredits, 10);
            Console.WriteLine(string.Join(Environment.NewLine, top10Actors.Select(a => $"{a.ActorName} - {a.MovieCount} фильмов")));

            Console.WriteLine("\nЗадание 5: Уникальные департаменты");
            var uniqueDepartments = GetUniqueDepartments(movieCredits);
            Console.WriteLine(string.Join(Environment.NewLine, uniqueDepartments));

            Console.WriteLine("\nЗадание 6: Фильмы с музыкой Ханса Циммера");
            var zimmerMovies = GetMoviesByComposer(movieCredits, "Hans Zimmer");
            Console.WriteLine(string.Join(Environment.NewLine, zimmerMovies));

            Console.WriteLine("\nЗадание 7: Словарь [ID фильма -> Режиссер] (первые 10)");
            var directorDictionary = CreateDirectorDictionary(movieCredits);
            foreach (var entry in directorDictionary.Take(10))
            {
                Console.WriteLine($"ID: {entry.Key}, Режиссер: {entry.Value}");
            }

            Console.WriteLine("\nЗадание 8: Фильмы, где снимались Брэд Питт и Джордж Клуни");
            var pittClooneyMovies = GetMoviesWithBothActors(movieCredits, "Brad Pitt", "George Clooney");
            Console.WriteLine(string.Join(Environment.NewLine, pittClooneyMovies));

            Console.WriteLine("\nЗадание 9: Всего человек в департаменте 'Camera'");
            var cameraDepartmentCount = GetDepartmentPersonCount(movieCredits, "Camera");
            Console.WriteLine($"Всего записей в департаменте 'Camera': {cameraDepartmentCount}");

            Console.WriteLine("\nЗадание 10: Люди, бывшие и актерами, и съемочной группой в 'Титанике'");
            var titanicCrewAndCast = GetPeopleWithDualRolesInMovie(movieCredits, "Titanic");
            Console.WriteLine(string.Join(Environment.NewLine, titanicCrewAndCast));

            Console.WriteLine("\nЗадание 11: Топ-5 соратников Квентина Тарантино (не актеры)");
            var tarantinoInnerCircle = GetDirectorInnerCircle(movieCredits, "Quentin Tarantino", 5);
            Console.WriteLine(string.Join(Environment.NewLine, tarantinoInnerCircle.Select(p => $"{p.Name} - {p.Count} раз")));

            Console.WriteLine("\nЗадание 12: Топ-10 самых частых экранных дуэтов");
            var actorPairs = GetTopActorDuos(movieCredits, 10);
            Console.WriteLine(string.Join(Environment.NewLine, actorPairs.Select(p => $"{p.Pair} - {p.Count} раз")));

            Console.WriteLine("\nЗадание 13: Топ-5 членов съемочной группы по числу разных департаментов");
            var mostDiverseCrew = GetMostDiverseCrewMembers(movieCredits, 5);
            Console.WriteLine(string.Join(Environment.NewLine, mostDiverseCrew.Select(p => $"{p.Name} - {p.DepartmentCount} департаментов")));

            Console.WriteLine("\nЗадание 14: Фильмы, где один человек был режиссером, сценаристом и продюсером");
            var creativeTrios = GetCreativeTrioMovies(movieCredits);
            Console.WriteLine(string.Join(Environment.NewLine, creativeTrios));

            Console.WriteLine("\nЗадание 15: Актеры в двух шагах от Кевина Бейкона (первые 20)");
            var secondDegreeActors = GetActorsTwoStepsFromKevinBacon(movieCredits);
            Console.WriteLine(string.Join(", ", secondDegreeActors.Take(20)));

            Console.WriteLine("\nЗадание 16: Средний размер команды для каждого режиссера (топ-10 по числу фильмов)");
            var teamWorkAnalysis = AnalyzeDirectorTeams(movieCredits, 10);
            foreach (var item in teamWorkAnalysis)
            {
                Console.WriteLine($"{item.Director} ({item.MovieCount} фильмов): ср. актеров - {item.AvgCastSize:F1}, ср. съемочная группа - {item.AvgCrewSize:F1}");
            }

            Console.WriteLine("\nЗадание 17: Основной департамент для людей, бывших и актерами, и в съемочной группе (топ-10)");
            var universalTalents = GetUniversalPeopleCareerPaths(movieCredits, 10);
            Console.WriteLine(string.Join(Environment.NewLine, universalTalents.Select(p => $"{p.Name} -> {p.MostFrequentDept}")));

            Console.WriteLine("\nЗадание 18: Люди, работавшие и со Скорсезе, и с Ноланом");
            var eliteClubMembers = GetEliteClubMembers(movieCredits, "Martin Scorsese", "Christopher Nolan");
            Console.WriteLine(string.Join(Environment.NewLine, eliteClubMembers));

            Console.WriteLine("\nЗадание 19: Ранжирование департаментов по среднему размеру актерского состава");
            var departmentInfluence = AnalyzeDepartmentInfluence(movieCredits);
            Console.WriteLine(string.Join(Environment.NewLine, departmentInfluence.Select(d => $"{d.Department}: {d.AvgCastSize:F1}")));

            Console.WriteLine("\nЗадание 20: 'Архетипы' персонажей Джонни Деппа");
            var deppArchetypes = AnalyzeJohnnyDeppArchetypes(movieCredits);
            Console.WriteLine(string.Join(Environment.NewLine, deppArchetypes.Select(a => $"{a.Archetype}: {a.Count} раз")));
        }


        public static List<string> GetMoviesByDirector(IReadOnlyList<MovieCredit> movieCredits, string directorName)
        {
            return movieCredits
                .Where(movie => movie.Crew.Any(crewMember => crewMember.Name == directorName && crewMember.Job == "Director"))
                .Select(movie => movie.Title)
                .ToList();
        }

        public static List<string> GetCharactersByActor(IReadOnlyList<MovieCredit> movieCredits, string actorName)
        {
            return movieCredits
                .SelectMany(movie => movie.Cast)
                .Where(castMember => castMember.Name == actorName)
                .Select(castMember => castMember.Character)
                .ToList();
        }

        public static List<MovieCredit> GetTopMoviesByCastSize(IReadOnlyList<MovieCredit> movieCredits, int count)
        {
            return movieCredits
                .OrderByDescending(movie => movie.Cast.Count)
                .Take(count)
                .ToList();
        }

        public static List<(string ActorName, int MovieCount)> GetTopActorsByMovieCount(IReadOnlyList<MovieCredit> movieCredits, int count)
        {
            return movieCredits
                .SelectMany(movie => movie.Cast)
                .GroupBy(castMember => castMember.Name)
                .Select(group => (ActorName: group.Key, MovieCount: group.Count()))
                .OrderByDescending(actor => actor.MovieCount)
                .Take(count)
                .ToList();
        }

        public static List<string> GetUniqueDepartments(IReadOnlyList<MovieCredit> movieCredits)
        {
            return movieCredits
                .SelectMany(movie => movie.Crew)
                .Select(crewMember => crewMember.Department)
                .Distinct()
                .OrderBy(dept => dept)
                .ToList();
        }

        public static List<string> GetMoviesByComposer(IReadOnlyList<MovieCredit> movieCredits, string composerName)
        {
            return movieCredits
                .Where(movie => movie.Crew.Any(c => c.Name == composerName && c.Job == "Original Music Composer"))
                .Select(movie => movie.Title)
                .ToList();
        }

        public static Dictionary<int, string> CreateDirectorDictionary(IReadOnlyList<MovieCredit> movieCredits)
        {
            return movieCredits
                .ToDictionary(
                    movie => movie.MovieId,
                    movie => movie.Crew.FirstOrDefault(c => c.Job == "Director")?.Name ?? "Не указан"
                );
        }

        public static List<string> GetMoviesWithBothActors(IReadOnlyList<MovieCredit> movieCredits, string actor1, string actor2)
        {
            return movieCredits
                .Where(movie => movie.Cast.Any(c => c.Name == actor1) && movie.Cast.Any(c => c.Name == actor2))
                .Select(movie => movie.Title)
                .ToList();
        }

        public static int GetDepartmentPersonCount(IReadOnlyList<MovieCredit> movieCredits, string department)
        {
            return movieCredits
                .SelectMany(movie => movie.Crew)
                .Count(crewMember => crewMember.Department == department);
        }

        public static List<string> GetPeopleWithDualRolesInMovie(IReadOnlyList<MovieCredit> movieCredits, string movieTitle)
        {
            var movie = movieCredits.FirstOrDefault(m => m.Title == movieTitle);
            if (movie == null)
            {
                Console.WriteLine($"Фильм '{movieTitle}' не найден.");
                return new List<string>();
            }

            var titanicCastIds = movie.Cast.Select(c => c.Id).ToHashSet();
            return movie.Crew
                .Where(crewMember => titanicCastIds.Contains(crewMember.Id))
                .Select(crewMember => crewMember.Name)
                .Distinct()
                .ToList();
        }

        public static List<(string Name, int Count)> GetDirectorInnerCircle(IReadOnlyList<MovieCredit> movieCredits, string directorName, int count)
        {
            var directorMoviesIds = movieCredits
                .Where(m => m.Crew.Any(c => c.Name == directorName && c.Job == "Director"))
                .Select(m => m.MovieId)
                .ToHashSet();

            return movieCredits
                .Where(m => directorMoviesIds.Contains(m.MovieId))
                .SelectMany(m => m.Crew)
                .Where(c => c.Name != directorName)
                .GroupBy(c => c.Name)
                .Select(g => (Name: g.Key, Count: g.Count()))
                .OrderByDescending(x => x.Count)
                .Take(count)
                .ToList();
        }

        public static List<(string Pair, int Count)> GetTopActorDuos(IReadOnlyList<MovieCredit> movieCredits, int count)
        {
            var actorPairs = movieCredits
                .SelectMany(movie =>
                    from actor1 in movie.Cast
                    from actor2 in movie.Cast
                    where actor1.Id < actor2.Id
                    select string.Compare(actor1.Name, actor2.Name) < 0
                        ? $"{actor1.Name} & {actor2.Name}"
                        : $"{actor2.Name} & {actor1.Name}"
                )
                .GroupBy(pair => pair)
                .Select(g => (Pair: g.Key, Count: g.Count()))
                .OrderByDescending(x => x.Count)
                .Take(count)
                .ToList();

            return actorPairs;
        }

        public static List<(string Name, int DepartmentCount)> GetMostDiverseCrewMembers(IReadOnlyList<MovieCredit> movieCredits, int count)
        {
            return movieCredits
                .SelectMany(m => m.Crew)
                .GroupBy(c => c.Name)
                .Select(g => (Name: g.Key, DepartmentCount: g.Select(c => c.Department).Distinct().Count()))
                .OrderByDescending(x => x.DepartmentCount)
                .Take(count)
                .ToList();
        }

        public static List<string> GetCreativeTrioMovies(IReadOnlyList<MovieCredit> movieCredits)
        {
            return movieCredits
                .Where(m => m.Crew
                    .GroupBy(c => c.Name)
                    .Any(g => g.Select(c => c.Job).Contains("Director") &&
                              g.Select(c => c.Job).Contains("Writer") &&
                              g.Select(c => c.Job).Contains("Producer")))
                .Select(m => m.Title)
                .ToList();
        }

        public static List<string> GetActorsTwoStepsFromKevinBacon(IReadOnlyList<MovieCredit> movieCredits)
        {
            var baconMoviesIds = movieCredits
                .Where(m => m.Cast.Any(c => c.Name == "Kevin Bacon"))
                .Select(m => m.MovieId)
                .ToHashSet();

            var firstDegreeActors = movieCredits
                .Where(m => baconMoviesIds.Contains(m.MovieId))
                .SelectMany(m => m.Cast)
                .Select(c => c.Name)
                .ToHashSet();

            var secondDegreeMoviesIds = movieCredits
                .Where(m => m.Cast.Any(c => firstDegreeActors.Contains(c.Name)))
                .Select(m => m.MovieId)
                .ToHashSet();

            return movieCredits
                .Where(m => secondDegreeMoviesIds.Contains(m.MovieId))
                .SelectMany(m => m.Cast)
                .Select(c => c.Name)
                .Except(firstDegreeActors)
                .Distinct()
                .ToList();
        }

        public static List<(string Director, int MovieCount, double AvgCastSize, double AvgCrewSize)> AnalyzeDirectorTeams(IReadOnlyList<MovieCredit> movieCredits, int count)
        {
            return movieCredits
                .Select(m => new { Director = m.Crew.FirstOrDefault(c => c.Job == "Director")?.Name, Movie = m })
                .Where(x => x.Director != null)
                .GroupBy(x => x.Director)
                .Select(g => (
                    Director: g.Key,
                    MovieCount: g.Count(),
                    AvgCastSize: g.Average(x => x.Movie.Cast.Count),
                    AvgCrewSize: g.Average(x => x.Movie.Crew.Count)
                ))
                .OrderByDescending(x => x.MovieCount)
                .Take(count)
                .ToList();
        }

        public static List<(string Name, string MostFrequentDept)> GetUniversalPeopleCareerPaths(IReadOnlyList<MovieCredit> movieCredits, int count)
        {
            var allCastIds = movieCredits.SelectMany(m => m.Cast).Select(c => c.Id).ToHashSet();
            return movieCredits
                .SelectMany(m => m.Crew)
                .Where(c => allCastIds.Contains(c.Id))
                .GroupBy(c => c.Name)
                .Select(g => (
                    Name: g.Key,
                    MostFrequentDept: g.GroupBy(c => c.Department)
                                        .OrderByDescending(deptGroup => deptGroup.Count())
                                        .First().Key
                ))
                .Take(count)
                .ToList();
        }

        public static List<string> GetEliteClubMembers(IReadOnlyList<MovieCredit> movieCredits, string director1, string director2)
        {
            var scorseseCrew = movieCredits
                .Where(m => m.Crew.Any(c => c.Name == director1 && c.Job == "Director"))
                .SelectMany(m => m.Crew.Select(c => c.Name).Concat(m.Cast.Select(c => c.Name)))
                .ToHashSet();

            var nolanCrew = movieCredits
                .Where(m => m.Crew.Any(c => c.Name == director2 && c.Job == "Director"))
                .SelectMany(m => m.Crew.Select(c => c.Name).Concat(m.Cast.Select(c => c.Name)))
                .ToHashSet();

            scorseseCrew.IntersectWith(nolanCrew);
            return scorseseCrew.ToList();
        }

        public static List<(string Department, double AvgCastSize)> AnalyzeDepartmentInfluence(IReadOnlyList<MovieCredit> movieCredits)
        {
            return movieCredits
                .SelectMany(m => m.Crew.Select(c => new { c.Department, CastSize = m.Cast.Count }))
                .GroupBy(x => x.Department)
                .Select(g => (Department: g.Key, AvgCastSize: g.Average(x => x.CastSize)))
                .OrderByDescending(x => x.AvgCastSize)
                .ToList();
        }

        public static List<(string Archetype, int Count)> AnalyzeJohnnyDeppArchetypes(IReadOnlyList<MovieCredit> movieCredits)
        {
            return movieCredits
                .SelectMany(m => m.Cast)
                .Where(c => c.Name == "Johnny Depp" && !string.IsNullOrWhiteSpace(c.Character))
                .Select(c => c.Character.Split(' ')[0])
                .GroupBy(archetype => archetype)
                .Select(g => (Archetype: g.Key, Count: g.Count()))
                .OrderByDescending(x => x.Count)
                .Where(x => x.Count > 1)
                .ToList();
        }
    }
}
