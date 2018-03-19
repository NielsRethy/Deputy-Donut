using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ButtonMaster : MonoBehaviour
{

    private enum CarType_e
    {
        POLICECAR,
        DONUTTRUCK
    }
    private struct Player_t
    {
        CarType_e CarType;

        public string PlayerHorizontalAxesKeyboard;
        public string PlayerHorizontalAxesController;
        public string PlayerVerticalAxesKeyboard;
        public string PlayerVerticalAxesControllerForward;
        public string PlayerVerticalAxesControllerBackward;
        public string PlayerBrake;
        public string PlayerNitro;
        public string PlayerAbility;
        public string PlayerReset;
        public string PlayerCameraFlip;
        public string PlayerCameraTurnToPlayer;
    }

 

    public static SCR_ButtonMaster Master;
    // Use this for initialization

    [Header("Player 1", order = 0)]
    public string Player1HorizontalAxesKeyboard = "Player1HorizontalAxesKeyboard";
    public string Player1HorizontalAxesController = "Player1HorizontalAxesController";
    public string Player1VerticalAxesKeyboard = "Player1VerticalAxesKeyboard";
    public string Player1VerticalAxesControllerForward = "Player1VerticalAxesControllerForward";
    public string Player1VerticalAxesControllerBackward = "Player1VerticalAxesControllerBackward";
    public string Player1Brake = "Player1Brake";
    public string Player1Nitro = "Player1Nitro";
    public string Player1Drift = "Player1Drift";
    public string Player1Ability = "Player1Ability";
    public string Player1Reset = "Player1Reset";
    public string Player1Ability2 = "Player1Ability2";
    public string Player1CameraFlip = "Player1FlipCamera";
    public string Player1CameraTurnToPlayer = "Player1CameraTurnToPlayer";
    public string Player1Pauze = "Player1Pauze";
    public static string Player1 = "Police";

    [Header("PLAYER2", order = 1)]
    public string Player2HorizontalAxesKeyboard = "Player2HorizontalAxesKeyboard";
    public string Player2HorizontalAxesController = "Player2HorizontalAxesController";
    public string Player2VerticalAxesKeyboard = "Player2VerticalAxesKeyboard";
    public string Player2VerticalAxesControllerForward = "Player2VerticalAxesControllerForward";
    public string Player2VerticalAxesControllerBackward = "Player2VerticalAxesControllerBackward";
    public string Player2Brake = "Player2Brake";
    public string Player2Nitro = "Player2Nitro";
    public string Player2Drift = "Player2Drift";
    public string Player2Ability = "Player2Ability";
    public string Player2Reset = "Player2Reset";
    public string Player2Ability2 = "Player2Ability2";
    public string Player2CameraFlip = "Player2FlipCamera";
    public string Player2CameraTurnToPlayer = "Player2CameraTurnToPlayer";
    public string Player2Pauze = "Player2Pauze";
    public static string Player2 = "Truck";

    [Header("AZERTY", order = 2)]
    public string Player1HorizontalAxesKeyboardAZERTY = "Player1HorizontalAxesKeyboardAZERTY";
    public string Player1VerticalAxesKeyboardAZERTY = "Player1VerticalAxesKeyboardAZERTY";
    public string Player1ResetAZERTY = "Player1ResetAZERTY";
    public string Player2BrakeAZERTY = "Player2BrakeAZERTY";
    public string Player2Ability2AZERTY = "Player2Ability2AZERTY";

    public bool _isKeyboardAzerty = false;

    private string _keyboardSwitchPlayer1HorizontalAxesKeyboard;
    private string _keyboardSwitchPlayer1VerticalAxesKeyboard;
    private string _keyboardSwitchPlayer1Reset;
    private string _keyboardSwitchPlayer2Brake;
    private string _keyboardSwitchPlayer2Ability2;

    public bool IsKeyboardAzerty
    {
        get { return _isKeyboardAzerty; }
        set
        {
            if (value)
            {
                Player1HorizontalAxesKeyboard = Player1HorizontalAxesKeyboardAZERTY;
                Player1VerticalAxesKeyboard = Player1VerticalAxesKeyboardAZERTY;
                Player1Reset = Player1ResetAZERTY;
                Player2Brake = Player2BrakeAZERTY;
                Player2Ability2 = Player2Ability2AZERTY;
            }
            else
            {
                Player1HorizontalAxesKeyboard = _keyboardSwitchPlayer1HorizontalAxesKeyboard;
                Player1VerticalAxesKeyboard = _keyboardSwitchPlayer1VerticalAxesKeyboard;
                Player1Reset = _keyboardSwitchPlayer1Reset;
                Player2Brake = _keyboardSwitchPlayer2Brake;
                Player2Ability2 = _keyboardSwitchPlayer2Ability2;
            }
            _isKeyboardAzerty = value;
        }
    }


    public string AcitvateAbility1 (bool Ability1)
    {
        if (Ability1)
        {
            return Player1Ability;
        }
        return Player1Ability2;
    }
    public string AcitvateAbility2(bool Ability1)
    {
        if (Ability1)
        {
            return Player2Ability;
        }
        return Player2Ability2;
    }

    void Start()
    {

        _keyboardSwitchPlayer1HorizontalAxesKeyboard = Player1HorizontalAxesKeyboard;
        _keyboardSwitchPlayer1VerticalAxesKeyboard = Player1VerticalAxesKeyboard;
        _keyboardSwitchPlayer1Reset = Player1Reset;
        _keyboardSwitchPlayer2Brake = Player2Brake;
        _keyboardSwitchPlayer2Ability2 = Player2Ability2;

        if (_isKeyboardAzerty)
        {
            Player1HorizontalAxesKeyboard = Player1HorizontalAxesKeyboardAZERTY;
            Player1VerticalAxesKeyboard = Player1VerticalAxesKeyboardAZERTY;
            Player1Reset = Player1ResetAZERTY;
            Player2Brake = Player2BrakeAZERTY;
            Player2Ability2 = Player2Ability2AZERTY;
        }
    }

    void Awake()
    {
        if (Master == null)
        {
            DontDestroyOnLoad(gameObject);
            Master = this;
        }
        else if (Master != this)
        {
            Destroy(gameObject);
        }
    }
}
