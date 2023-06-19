using OfficeOpenXml;

namespace BackEnd;

public class Algorithm
{
    class Program
    {
        static void Main()
        {
            string filePath = "C:/Users/luisv/Downloads/dados (1).xlsx";

            List<Dictionary<string, string>> teamMembers = ReadDataFromFile(filePath);

            List<Dictionary<string, string>> initialTeam = GenerateTeam(5, teamMembers);

            List<Dictionary<string, string>> bestSolution = initialTeam;
            double bestScore = EvaluateBalance(bestSolution);

            for (int i = 1; i <= 1000; i++)
            {
                List<List<Dictionary<string, string>>> expandedNeighborhood = ExpandNeighborhood(bestSolution);

                List<Dictionary<string, string>> newSolution = SelectBestTeam(bestSolution, expandedNeighborhood);
                double newScore = EvaluateBalance(newSolution);

                if (newScore < bestScore)
                {
                    bestSolution = newSolution;
                    bestScore = newScore;
                    Console.WriteLine($"Iteração: {i}, Avaliação: {bestScore}");
                }
                else
                {
                    Console.WriteLine("Nenhuma melhoria encontrada");
                }
            }

            Console.WriteLine("Melhor solução encontrada:");
            PrintTeam(bestSolution);
        }

        static List<Dictionary<string, string>> ReadDataFromFile(string filePath)
        {
            List<Dictionary<string, string>> teamMembers = new List<Dictionary<string, string>>();

            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                List<string> headers = new List<string>();

                for (int col = 1; col <= colCount; col++)
                {
                    headers.Add(worksheet.Cells[1, col].Value.ToString());
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    Dictionary<string, string> member = new Dictionary<string, string>();

                    for (int col = 1; col <= colCount; col++)
                    {
                        string header = headers[col - 1];
                        string value = worksheet.Cells[row, col].Value.ToString();

                        member.Add(header, value);
                    }

                    teamMembers.Add(member);
                }
            }

            return teamMembers;
        }

        static List<Dictionary<string, string>> GenerateTeam(int size, List<Dictionary<string, string>> teamMembers)
        {
            Random random = new Random();

            List<Dictionary<string, string>> team = new List<Dictionary<string, string>>();

            List<string> roles = teamMembers.Select(member => member["Papel"]).Distinct().ToList();

            foreach (string role in roles)
            {
                Dictionary<string, string> member = teamMembers.First(m => m["Papel"] == role);
                team.Add(member);
            }

            int juniorCount = team.Count(member => member["Nível"] == "junior");

            while (team.Count < size)
            {
                double juniorProbability = (double)juniorCount / team.Count;

                if (random.NextDouble() <= juniorProbability)
                {
                    List<string> juniorRoles = roles.Where(role => teamMembers.Any(member => member["Nível"] == "junior" && member["Papel"] == role)).ToList();
                    string selectedRole = juniorRoles[random.Next(juniorRoles.Count)];

                    Dictionary<string, string> member = teamMembers.First(m => m["Papel"] == selectedRole && m["Nível"] == "junior");
                    team.Add(member);

                    juniorCount++;
                }
                else
                {
                    string selectedRole = roles[random.Next(roles.Count)];

                    Dictionary<string, string> member = teamMembers.First(m => m["Papel"] == selectedRole);
                    team.Add(member);
                }
            }

            while (team.Select(member => member["Nome"]).Distinct().Count() < team.Count)
            {
                team = GenerateTeam(size, teamMembers); // Reinicia o processo se houver membros duplicados
            }

            return team;
        }

        static List<List<Dictionary<string, string>>> ExpandNeighborhood(List<Dictionary<string, string>> initialTeam)
        {
            List<List<Dictionary<string, string>>> expandedNeighborhood = new List<List<Dictionary<string, string>>>();

            foreach (Dictionary<string, string> member in initialTeam)
            {
                List<Dictionary<string, string>> expandedTeam = new List<Dictionary<string, string>>(initialTeam);

                string role = member["Papel"];
                expandedTeam.Remove(member);

                List<Dictionary<string, string>> availableMembers = expandedTeam.Where(m => m["Papel"] == role).ToList();

                if (availableMembers.Count > 1)
                {
                    availableMembers.RemoveAt(0);

                    foreach (Dictionary<string, string> newMember in availableMembers)
                    {
                        List<Dictionary<string, string>> expandedTeamCopy = new List<Dictionary<string, string>>(expandedTeam);
                        expandedTeamCopy.Add(newMember);

                        expandedNeighborhood.Add(expandedTeamCopy);
                    }
                }
            }

            return expandedNeighborhood;
        }

        static double EvaluateBalance(List<Dictionary<string, string>> team)
        {
            double juniorPercentage = (double)team.Count(member => member["Nível"] == "junior") / team.Count;

            if (juniorPercentage == 1)
            {
                return 1.0; // Pior avaliação se todos os membros forem juniores
            }
            else if (juniorPercentage > 0.5)
            {
                double juniorDifference = Math.Abs(juniorPercentage - 0.5);
                double plenoSeniorDifference = Math.Abs((team.Count(member => member["Nível"] == "pleno") + team.Count(member => member["Nível"] == "senior")) - 0.5);

                return juniorDifference + plenoSeniorDifference;
            }
            else
            {
                double juniorDifference = Math.Abs(juniorPercentage - 0.5);
                double plenoSeniorDifference = Math.Abs((team.Count(member => member["Nível"] == "pleno") + team.Count(member => member["Nível"] == "senior")) - team.Count(member => member["Nível"] == "junior"));

                double bonus = (team.Any(member => member["Nível"] == "pleno" || member["Nível"] == "senior")) ? 0.5 : 0;

                return juniorDifference + plenoSeniorDifference + bonus;
            }
        }

        static List<Dictionary<string, string>> SelectBestTeam(List<Dictionary<string, string>> initialTeam, List<List<Dictionary<string, string>>> expandedNeighborhood)
        {
            List<Dictionary<string, string>> bestTeam = initialTeam;
            double bestScore = EvaluateBalance(bestTeam);

            foreach (List<Dictionary<string, string>> team in expandedNeighborhood)
            {
                double score = EvaluateBalance(team);

                if (score < bestScore)
                {
                    bestTeam = team;
                    bestScore = score;
                }
            }

            return bestTeam;
        }

        static void PrintTeam(List<Dictionary<string, string>> team)
        {
            Console.WriteLine("Membros da equipe:");

            foreach (Dictionary<string, string> member in team)
            {
                Console.WriteLine($"Nome: {member["Nome"]}, Papel: {member["Papel"]}, Nível: {member["Nível"]}, Linguagem: {member["Linguagem"]}");
            }
        }
    }
}