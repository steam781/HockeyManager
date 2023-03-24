using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // define teams and players
        Player[] homePlayers = {
            new Player("Forward 1", 10, 100),
            new Player("Forward 2", 10, 100),
            new Player("Midfielder 1", 20, 100),
            new Player("Defebder 1", 20, 100),
            new Player("Defender 2", 30, 100),
            new Goalie("Goalie", 50)
};
        Team homeTeam = new Team("Home", homePlayers);

        Player[] awayPlayers = {
            new Player("Forward 1", 10, 100),
            new Player("Forward 2", 10, 100),
            new Player("Midfielder 1", 20, 100),
            new Player("Defebder 1", 20, 100),
            new Player("Defender 2", 30, 100),
            new Goalie("Goalie", 50)
        };
        Team awayTeam = new Team("Away", awayPlayers);

        // start match
        Match match = new Match(homeTeam, awayTeam, 90);
        match.Play();

        // print final score
        Console.WriteLine($"{homeTeam.name} {match.homeScore} - {match.awayScore} {awayTeam.name}");
    }
}

public class Player
{
    public string Name { get; }
    public int PE { get; }
    public int Stamina { get; private set; }

    // Statistics properties
    public int Shots { get; set; }
    public int Goals { get; set; }
    public int Penalties { get; set; }

    public Player(string name, int pe)
    {
        Name = name;
        PE = pe;
        Stamina = 100;
    }

    public double CalculateShotChance(int strategy, int LPE)
    {
        double baseChance = (double)PE / 100;
        double strategyModifier = GetStrategyModifier(strategy);
        double staminaModifier = GetStaminaModifier();

        double totalChance = baseChance * strategyModifier * staminaModifier * LPE;

        return totalChance;
    }

    private double GetStrategyModifier(int strategy)
    {
        switch (strategy)
        {
            case 0:
                return 0.8; // Defense
            case 1:
                return 1.2; // Attack
            default:
                return 1.0; // Neutral
        }
    }

    private double GetStaminaModifier()
    {
        double staminaFraction = (double)Stamina / 100;

        if (staminaFraction >= 0.5)
        {
            return 1.0; // No effect if stamina is >= 50%
        }
        else if (staminaFraction >= 0.2)
        {
            return 0.8; // 20% reduction if stamina is < 50% but >= 20%
        }
        else
        {
            return 0.5; // 50% reduction if stamina is < 20%
        }
    }

    public void DrainStamina(int strategy)
    {
        switch (strategy)
        {
            case 0:
                Stamina -= 2; // Defense
                break;
            case 1:
                Stamina -= 3; // Attack
                break;
            default:
                Stamina -= 2; // Neutral
                break;
        }

        if (Stamina < 0)
        {
            Stamina = 0;
        }
    }
}

public class Goalie : Player
{
    public int Saves { get; set; }

    public Goalie(string name, int pe) : base(name, pe)
    {
        Saves = 0;
    }

    public double CalculateSaveChance(int strategy, int LPE)
    {
        double baseChance = (double)PE / 100;
        double strategyModifier = GetStrategyModifier(strategy);
        double staminaModifier = GetStaminaModifier();

        double totalChance = baseChance * strategyModifier * staminaModifier * LPE;

        return totalChance;
    }
}

public class Team
{
    public string name { get; set; }
    public List<Player> players { get; set; }
    public int LPE { get; set; }
    public int ShotsAgainst { get; set; }  // Shots against this team
    public int GoalsAgainst { get; set; }  // Goals scored against this team

    public Team(string name, Player[] players)
    {
        this.name = name;
        this.players = players;
    }

    public int GetLPE()
    {
        int lpe = 0;
        foreach (Player player in players)
        {
            lpe += player.PE;
        }
        return lpe;
    }

    public void ApplyStaminaLoss(bool isOnAttack)
    {
        int staminaLossPerMinute = isOnAttack ? 3 : 1; // higher stamina loss when on Attack strategy
        foreach (Player player in players)
        {
            player.stamina -= staminaLossPerMinute;
        }
    }
}

public class Match
{
    public Team homeTeam;
    public Team awayTeam;
    public int timeLimit;
    public int homeScore;
    public int awayScore;

    private int currentTime;
    private int currentPossession;
    private int homeLPE;
    private int awayLPE;
    private int homeNumPlayers;
    private int awayNumPlayers;

    private Random random;

    public Match(Team homeTeam, Team awayTeam, int timeLimit)
    {
        this.homeTeam = homeTeam;
        this.awayTeam = awayTeam;
        this.timeLimit = timeLimit;
        this.homeNumPlayers = homeTeam.players.Length;
        this.awayNumPlayers = awayTeam.players.Length;
        this.random = new Random();
    }

    public void Play()
    {
        Console.WriteLine($"Match between {homeTeam.name} and {awayTeam.name} begins!");

        homeLPE = homeTeam.GetLPE();
        awayLPE = awayTeam.GetLPE();
        currentPossession = random.Next(2); // randomly choose which team has possession at the start

        while (currentTime < timeLimit)
        {
            if (random.Next(100) == 0)
            {
                // Penalty: one player is sent off for 2 minutes
                Team penalizedTeam;
                int penalizedPlayerIndex;

                if (currentPossession == 0)
                {
                    // Home team penalized
                    penalizedTeam = homeTeam;
                    penalizedPlayerIndex = random.Next(homeNumPlayers);
                    Console.WriteLine($"{homeTeam.name} player {penalizedPlayerIndex + 1} is sent off for 2 minutes!");

                    homeNumPlayers--;
                    homeLPE = homeTeam.GetLPE();
                }
                else
                {
                    // Away team penalized
                    penalizedTeam = awayTeam;
                    penalizedPlayerIndex = random.Next(awayNumPlayers);
                    Console.WriteLine($"{awayTeam.name} player {penalizedPlayerIndex + 1} is sent off for 2 minutes!");

                    awayNumPlayers--;
                    awayLPE = awayTeam.GetLPE();
                }

                // Play continues for 2 minutes with the penalized player off the ice
                for (int i = 0; i < 120; i++)
                {
                    PlayMinute();

                    if (currentTime >= timeLimit) break;

                    // Check if the penalized player can return to the ice
                    if (i == 119)
                    {
                        Console.WriteLine($"Player {penalizedPlayerIndex + 1} returns to the ice!");
                        penalizedTeam.players[penalizedPlayerIndex].isPenalized = false;

                        if (penalizedTeam == homeTeam)
                        {
                            homeNumPlayers++;
                            homeLPE = homeTeam.GetLPE();
                        }
                        else
                        {
                            awayNumPlayers++;
                            awayLPE = awayTeam.GetLPE();
                        }
                    }
                }
            }
            else
            {
                PlayMinute();
            }

            if (currentTime >= timeLimit) break;

            // Switch possession after each minute
            currentPossession = 1 - currentPossession;
            currentTime++;
        }

        Console.WriteLine($"Match between {homeTeam.name} and {awayTeam.name} ends!");
    }

    private void PlayMinute()
    {
        Console.WriteLine($"Minute {currentTime + 1}:");

        int teamWithPuck;
        if (currentPossession == 0)
        {
            // Home team has possession
            teamWithPuck = homeLPE > awayLPE ? 0 : 1;
        }
        else
        {
            // Away team has possession
            teamWithPuck = awayLPE > homeLPE ? 1 : 0;
        }

        // Check if there's a shot on goal

        // Choose attacking and defending teams
        Team attackingTeam = teamWithPuck == 0 ? homeTeam : awayTeam;
        Team defendingTeam = teamWithPuck == 0 ? awayTeam : homeTeam;

        // Shoot the puck
        attackingTeam.Shoot(attackingTeam, defendingTeam);

        // Choose a strategy for each team
        int homeStrategy = homeTeam.ChooseStrategy();
        int awayStrategy = awayTeam.ChooseStrategy();

        // Adjust LPE based on strategy
        if (homeStrategy == 0)
        {
            homeLPE -= homeTeam.GetStaminaDrain();
        }
        else if (homeStrategy == 2)
        {
            homeLPE += homeTeam.GetStaminaDrain();
        }

        if (awayStrategy == 0)
        {
            awayLPE -= awayTeam.GetStaminaDrain();
        }
        else if (awayStrategy == 2)
        {
            awayLPE += awayTeam.GetStaminaDrain();
        }

        // Adjust LPE based on penalty status
        homeLPE += homeTeam.GetPenaltyBoost();
        awayLPE += awayTeam.GetPenaltyBoost();
    }

    public void Shoot(Team attackingTeam, Team defendingTeam)
    {
        double shotChance = attackingTeam.CalculateShotChance(attackingTeam.Strategy, attackingTeam.LPE);
        double saveChance = defendingTeam.Goalie.CalculateSaveChance(defendingTeam.Strategy, defendingTeam.LPE);

        if (random.Next(100) < shotChance * 100)
        {
            attackingTeam.Shots++;  // Increment the number of shots for the attacking team
            defendingTeam.ShotsAgainst++;  // Increment the number of shots against for the defending team

            if (random.Next(100) < saveChance * 100)
            {
                defendingTeam.Goalie.Saves++; // Increment the number of saves for the defending team's goalie
                Console.WriteLine("{0} shoots, but {1} saves it.", attackingTeam.Name, defendingTeam.Goalie.Name);
            }
            else
            {
                attackingTeam.Goals++;  // Increment the number of goals for the attacking team
                defendingTeam.GoalsAgainst++;  // Increment the number of goals against for the defending team
                Console.WriteLine("GOAL! {0} scores!", attackingTeam.Name);
            }
        }
        else
        {
            Console.WriteLine("{0} tries to shoot, but {1} defends it.", attackingTeam.Name, defendingTeam.Name);
        }

        // Drain stamina for players on the attacking team
        foreach (var player in attackingTeam.Players)
        {
            if (player.Stamina > 0)
            {
                player.Stamina -= 1;
            }
        }
    }

    public void Penalty(Player player, Team team)
    {
        player.Penalties++;  // Increment the number of penalties for the player
        team.ShotsAgainst++;  // Increment the number of shots against for the player's team
        team.GoalsAgainst++;  // Increment the number of goals against for the player's team
        Console.WriteLine("{0} gets a penalty!", player.PE);

        // Temporarily remove the player's PE from the team's LPE
        team.LPE -= player.PE;

        // Set a timer to restore the player's PE after the penalty duration
        var timer = new Timer();
        timer.Interval = penaltyDuration * 1000;  // Convert seconds to milliseconds
        timer.Elapsed += (sender, e) =>
        {
            // Restore the player's PE and update the team's LPE
            player.Stamina -= 20;  // Reduce player's stamina as penalty for the duration
            team.LPE += player.PE;
            Console.WriteLine("{0} is back on the ice!", player.PE);
        };
        timer.AutoReset = false;
        timer.Start();
    }
    public void DisplayStatistics()
    {
        Console.WriteLine();
        Console.WriteLine("MATCH STATISTICS");
        Console.WriteLine("----------------");
        Console.WriteLine("{0} {1} - {2} {3}", teamA.Name, teamA.Goals, teamB.Goals, teamB.Name);
        Console.WriteLine();

        Console.WriteLine("TEAM STATISTICS");
        Console.WriteLine("---------------");
        Console.WriteLine("{0}: {1} shots, {2} goals, {3} penalties", teamA.Name, teamA.Shots, teamA.Goals, teamA.Players.Sum(p => p.Penalties));
        Console.WriteLine("{0}: {1} shots, {2} goals, {3} penalties", teamB.Name, teamB.Shots, teamB.Goals, teamB.Players.Sum(p => p.Penalties));
        Console.WriteLine();

        Console.WriteLine("PLAYER STATISTICS");
        Console.WriteLine("-----------------");
        foreach (var player in homeTeam.Players.Concat(awayTeam.Players))
        {
            Console.WriteLine($"{player.Name}: Shots-{player.Shots} Goals-{player.Goals} Penalties-{player.Penalties}");
        }
        foreach (var Goalie in homeTeam.Players.Concat(awayTeam.Players))
        {
            Console.WriteLine($"{player.Name}: Shots-{player.Shots} Goals-{player.Goals} Penalties-{player.Penalties}");
        }

        Console.WriteLine("\nTEAM STATISTICS");
        Console.WriteLine("----------------");
        Console.WriteLine($"{homeTeam.Name}: Shots Against-{awayTeamShots} Goals Against-{awayTeamGoals}");
        Console.WriteLine($"{awayTeam.Name}: Shots Against-{homeTeamShots} Goals Against-{homeTeamGoals}");
    }

    //In this implementation, there is a 1 in 10 chance that a penalty occurs each minute. If a penalty occurs, a player from the team with possession is randomly chosen to be sent off for 2 minutes. The player's LPE is temporarily removed from their team's total, and the team plays with one less player for the duration of the penalty.
    //During the penalty, the `PlayMinute` method is still called for each minute, but the penalized player's `isPenalized` property is set to `true`, which prevents them from participating in the game until the penalty time has expired. After the penalty time has elapsed, the player is allowed to return to the ice, and their LPE is added back to their team's total.
    //Note that the implementation of penalties is just one way to handle them, and there are many other ways you could design the game to include them.
}"