using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueLike : MonoBehaviour
{
    int avatarX = 0;
	int avatarY = 0;
	int levelWidth 	= 30;
	int levelHeight = 10;
	
	int state = 0;
	int STATE_DRAW_LEVEL = 0;
	int STATE_INPUT = 1;
    
    void Update()
    {
		if(state == STATE_DRAW_LEVEL)
		{
			// clear screen
			Console.Clear();
			
			// dessiner toutes les lignes (répéter X fois le dessin d'une ligne)
			for(int y=0; y<levelHeight; y++) 
			{
				// dessiner une ligne
				for(int x=0; x<levelWidth; x++) 
				{
					if(x == avatarX && y == avatarY)
					{
						Console.Print("a");
					}else
					{
						Console.Print(".");
					}
				}
				Console.Println("");
			}
			
			
			state = STATE_INPUT;
			
			
		}else if(state == STATE_INPUT)
		{
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				avatarX--;
				state = STATE_DRAW_LEVEL;
			}
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				avatarX++;
				state = STATE_DRAW_LEVEL;
			}
		}
    }
}
