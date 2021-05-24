using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text puntosPlayer;
    public Text puntosDealer;
    public int[] deckShuffle = new int[52];
    private int variableAux;

    public Button apostar;
    public Button restar;
    public Text betMessage;
    public Text BancaMessage;
    int banca = 1000;
    int apuesta = 0;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        int[] val = { 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 
                      11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 
                      11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 
                      11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = val[i];
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        for (int i = 0; i < deckShuffle.Length; i++)
        {
            int rnd = Random.Range(0, deckShuffle.Length);
            variableAux = rnd;
            deckShuffle[i] = variableAux;
        }
    }

    void StartGame()
    {
        apuesta = 0;
        modificarApuestas(); 

        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
        puntosPlayer.text = player.GetComponent<CardHand>().points.ToString();
        if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blacjack!";
            hitButton.interactable = false;
            stickButton.interactable = false;
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            banca = banca + (apuesta * 2);

            modificarApuestas();

        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[deckShuffle[cardIndex]], values[deckShuffle[cardIndex]]);
        cardIndex++;
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[deckShuffle[cardIndex]], values[deckShuffle[cardIndex]]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        puntosPlayer.text = player.GetComponent<CardHand>().points.ToString();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        if (player.GetComponent<CardHand>().points > 21)
        {
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "Has perdido";
            hitButton.interactable = false;
            stickButton.interactable = false;

            modificarApuestas();

        }
    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        hitButton.interactable = false;
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        while (dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer();
        }
        puntosDealer.text = dealer.GetComponent<CardHand>().points.ToString();

        if (dealer.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blacjack! Has perdido";
            modificarApuestas();

        }
        else if (dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Has ganado";
            banca = banca + (apuesta * 2);
            modificarApuestas();

        }
        else if (dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has ganado";
            banca = banca + (apuesta * 2);
            modificarApuestas();

        }
        else if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has empatado";
            banca = banca + apuesta;
            modificarApuestas();

        }
        else
        {
            finalMessage.text = "Has perdido";
            modificarApuestas();

        }
        stickButton.interactable = false;
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        puntosDealer.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
    public void sumarMultiplos10()
    {
        if( banca >= 10)
        {
            apuesta = apuesta + 10;
            banca = banca - 10;
            modificarApuestas(); // modifica los valores de la banca
        }
    }

    public void restarMultiplos10()
    {
        if (apuesta <= 0)
        {
            apuesta = 0;
        }

        else
        {
            apuesta = apuesta - 10;
            banca = banca + 10;
            modificarApuestas();
        }
        
    }

    public void modificarApuestas()
    {
        betMessage.text = apuesta.ToString();
        BancaMessage.text = banca.ToString();
    }
}
