using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTourParTour : MonoBehaviour
{
  // données de gameplay
  int playerHP        		= 10;
  int playerATK       		= 1;
  string enemyName    		= "Chaussure";
  
  int enemyHP         		= 20;
  int enemyMinATK     		= 1;
  int enemyMaxATK     		= 5;

  bool isPoisoned     		= false;
  int poisonTurnsCounter 	= 0; // compte le nombre de tours de poison
  int poisonDamagePerTurn 	= 2;

  int magicDamage 			= 5;
  int magicCooldown 		= 5;
  int magicCooldownCounter 	= 0;

  int criticalChance 		= 15; // pourcentage
  int criticalDamage 		= 8;
  
  bool defending 			= false;

  int nbPotion 				= 3;
  int potionHPgain 			= 7;

  
  // la variable gameState va permettre de dire dans quel phase du jeu on se trouve (attaque ennemi, choix du joueur, attaque joueur, etc)
  const int FIGHT_START 			= 0;
  const int PLAYER_PHASE_START 		= 1;
  const int WAIT_FOR_PLAYER_CHOICE 	= 2;
  const int ENEMY_PHASE 			= 3;
  const int TURN_START 				= 4;
  const int TURN_END 				= 5;
  const int WIN 					= 6;
  const int LOSE 					= 7;
  const int WAIT_BEFORE_RESTART		= 8;

  int gameState = FIGHT_START;

  
  void Update()
  {
    switch(gameState)
    {
      case FIGHT_START:
	  {
		// initialize variables (for combat restart)
		playerHP 				= 10;
		enemyHP 				= 20;
		isPoisoned     			= false;
		poisonTurnsCounter 		= 0; // compte le nombre de tours de poison
		magicCooldownCounter 	= 0;
		defending 				= false;
		nbPotion 				= 3;
	  
	  
        Console.Println("Un ennemi se présente face à vous. Il a l'air plutôt inoffensif...");
		Console.Println("");
        gameState = TURN_START;
	  }
      break;

      case TURN_START:
	  {
        Console.Println("===================================");
        Console.Println(string.Format("Player HP: {0}\t\t{1} HP: {2}", playerHP, enemyName, enemyHP));
        Console.Println("===================================");
		Console.Println("");

        // on décrémente le compteur de cooldown de magie à chaque tour
        magicCooldownCounter--;

        if(isPoisoned == true)
        {
          enemyHP -= poisonDamagePerTurn;
          Console.Println(string.Format("{0} vomit ses tripes et perd {1} PV", enemyName, poisonDamagePerTurn));
          poisonTurnsCounter++;
          if(poisonTurnsCounter >= 3)
          {
            isPoisoned = false;
            Console.Println(string.Format("{0} n'est plus empoisonné", enemyName));
          }
        }

        gameState = PLAYER_PHASE_START;
	  }
      break;

      case PLAYER_PHASE_START:
	  {
        Console.Println("===================================");
        Console.Println("Attaquer(A)  Defendre(D)  Potion(P)  Magie(M)  Attaque Critique(C)  Poison(K)\n");
        gameState = WAIT_FOR_PLAYER_CHOICE;
	  }
      break;

      case WAIT_FOR_PLAYER_CHOICE:
      {
        ////////// attaquer
        if(Input.GetKeyDown(KeyCode.A))
        {
          int damage = playerATK; // calcul des dommages infligés
          enemyHP -= damage;
          // les deux lignes de debug produisent la même sortie mais leur syntaxe est différente, à vous de choisir celle qui vous convient le plus
          Console.Println(string.Format("Vous attaquez {0} et lui infligez {1} dégats !", enemyName, damage));

          gameState = ENEMY_PHASE;
        }

        ////////// défendre
        if(Input.GetKeyDown(KeyCode.D))
        {
          defending = true;
          Console.Println("Vous tremblez comme une feuille en tenant votre bouclier devant vous.");
          gameState = ENEMY_PHASE;
        }

        ////////// potion
        if(Input.GetKeyDown(KeyCode.P))
        {
          if(nbPotion > 0)
          {
            nbPotion--;
            playerHP += potionHPgain;
            Console.Println("Ahhh, ça breuvage est bien ragaillardissant !");
            gameState = ENEMY_PHASE;
          }else
          {
            Console.Println("Vous fouillez dans votre poche pour vous rendre compte que vous n'avez plus de potions !");
            gameState = PLAYER_PHASE_START;
          }
        }

        ////////// attaque critique
        if(Input.GetKeyDown(KeyCode.C))
        {
          int randomNb = Random.Range(0, 101); // le maximum de Random.Range est MAX-1
          // if(RND < PROBA) => c'est bon
          if(randomNb < criticalChance)
          {
            Console.Println("Vous assénez un coup critique terrible ! " + criticalDamage + " énormes aiguilles transpercent "+enemyName+ " , lui infligeant chacune 1 point de dégât.");
            enemyHP -= criticalDamage;
          }else
          {
            Console.Println("Vous assénez un coup critique terrible... dans votre pied.");
          }
          gameState = ENEMY_PHASE;
        }


        ////////// magic
        if(Input.GetKeyDown(KeyCode.M))
        {
          if(magicCooldownCounter <= 0) // si magie dispo
          {
            Console.Println("Wildfire !!!! ("+magicDamage+" dégâts)");
            enemyHP -= magicDamage;
            magicCooldownCounter = magicCooldown;
            gameState = ENEMY_PHASE;
          }else
          {
            Console.Println("Vous n'avez plus d'allumettes.");
            gameState = PLAYER_PHASE_START;
          }
        }


        ////////// poison
        if(Input.GetKeyDown(KeyCode.K))
        {
          if(isPoisoned == false)
          {
            isPoisoned = true;
            poisonTurnsCounter = 0;
			Console.Println("Vous glissez une enveloppe à l'anthrax dans la boîte aux lettres de votre adversaire.");
            gameState = ENEMY_PHASE;
          }else
          {
            Console.Println("Votre ennemi est déjà en train de s'étouffer dans la cigüe, n'abusez pas.");
            gameState = PLAYER_PHASE_START;
          }
        }
      }

      if(Input.GetKeyDown(KeyCode.Q))
      {
        Application.Quit(); // pour quitter le jeu
      }

      if(enemyHP <= 0)
      {
        gameState = WIN;
      }
        
      break;


      case ENEMY_PHASE:
      {
        int damage = Random.Range(enemyMinATK, enemyMaxATK);
        if(defending)
        {
          damage = damage / 2;
          defending = false;
        }
        Console.Println(string.Format("{0} vous inflige {1} dégâts", enemyName, damage));
		Console.Println("");
        playerHP -= damage;

        if(playerHP <= 0)
        {
          gameState = LOSE;  
        }else
        {
			Console.Println("Fin du tour, appuyez sur n'importe quelle touche pour continuer...");
			gameState = TURN_END;
        }
      }
      break;
	  
	  case TURN_END:
	  {
		if(Input.anyKeyDown)
		{
			gameState = TURN_START;
			Console.Clear();
		}
	  }
	  break;


      case WIN:
	  {
        Console.Println("Vous avez vaincu les ténèbres. La félicité vous envahit. Une porte se dresse devant vous.... Vous l'ouvrez, et....");
        gameState = WAIT_BEFORE_RESTART; // should be TURN_START but we want to wait for input before begining again
	  }
      break;

      case LOSE:
	  {
        Console.Println("Vous vous écroulez lamentablement sur le sol. Votre ennemi ne daigne même pas vous uriner dessus.");
        Console.Println("Vous n'êtes pas mort cependant. Mais vous morflez grave. Vous ouvrez péniblement un oeil et...");
        gameState = WAIT_BEFORE_RESTART; // should be TURN_START but we want to wait for input before begining again
	  }
      break;
	  
	  case WAIT_BEFORE_RESTART :
	  {
		if(Input.anyKeyDown)
		{
			gameState = FIGHT_START;
			Console.Clear();
		}
	  }
	  break;
	  
    }
  }
}
