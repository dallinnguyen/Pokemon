using UnityEngine;
using System.Collections;
using UnityEditor;

public partial class ItemEditor : EditorWindow {
    MedicineDatabase database;

    const int SPRITE_BUTTON_SIZE = 46;
    const string FILE_NAME = @"ItemDatabase.asset";
    const string FOLDER_NAME = @"Database";
    const string DATABASE_PATH = @"Assets/" + FOLDER_NAME + "/" + FILE_NAME;

    bool isLogined = false;

    private Admin admin;
    private bool awaitingRedirect;

    private string user;
    private string pass;

    [MenuItem("IS/Database/Item System Editor %#i")]
    public static void Init()
    {
        ItemEditor window = EditorWindow.GetWindow<ItemEditor>();
        window.minSize = new Vector2(800, 600);
        window.title = "Item system";
        window.Show();

    }

    void OnEnable()
    {
        if (database == null)
        {
            database = MedicineDatabase.GetDatabase<MedicineDatabase>(FOLDER_NAME, FILE_NAME);
        }
        admin = Admin.Instance;
        admin.Initialize();
        admin.OnConnect += OnConnect;
        admin.OnDisconnect += OnDisconnect;
        admin.OnLoginFailure += OnLoginFailure;
        admin.OnAccount += OnAccount;
        admin.OnReceiveRedirect += OnReceiveRedirect;
        admin.OnDisconnecting += OnDisconnecting;
        
    }

    void OnDisable()
    {
        admin.Disconnect("closing editor");
    }

    void OnGUI()
    {
        if (isLogined)
        {
            TopTabBar();
            //GUILayout.Label("top tab bar");
            GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            ListView();
            //GUILayout.Label("List View");
            itemDetails();
            //GUILayout.Label("item details");
            GUILayout.EndHorizontal();
            StatusBar();
        }
        else
        {
            Login();
        } 
    }

    void Login()
    {
        
        user = EditorGUILayout.TextField("Username: ", user);
        pass = EditorGUILayout.TextField("Password: ", pass);
        if (GUILayout.Button("Login"))
        {
            admin.Login(user,pass);
            
        }
        if (GUILayout.Button("send test data"))
        {
            admin.testMessage();
        }
    }

    private void OnReceiveRedirect()
    {
        awaitingRedirect = true;
    }

    private void OnLoginFailure(string message)
    {
        UIController.Instance.ClosePopup(Popup.StatusPopup);
        TextCallback callback = new TextCallback(message);
        UIController.Instance.OpenPopup<bool>(Popup.TextPopup, callback);
        UIController.Instance.OpenUIWindow(UIWindow.Login);

        Debug.Log("Login failed: " + message);
    }

    private void OnDisconnecting()
    {
    }

    private void OnDisconnect(string message)
    {
        /*UIController.Instance.ClosePopup(Popup.StatusPopup);
        UIController.Instance.OpenUIWindow(UIWindow.Login);
        TextCallback callback = new TextCallback(message);
        UIController.Instance.OpenPopup<bool>(Popup.TextPopup, callback);*/

        Debug.Log("OnDisconnect: " + message);

        if (awaitingRedirect && admin.RedirectTarget != null && admin.Token != null && admin.Username != null && admin.CharacterId > 0)
        {
            Debug.Log("connecting");
            admin.Connect(admin.RedirectTarget, admin.Token, admin.Username, admin.CharacterId);
        }
        else if (awaitingRedirect)
        {
            Debug.Log("Error on connecting to node, parameters not set?");
        }
    }

    private void OnConnect()
    {
        Debug.Log("Connected!");
        isLogined = true;
        if (awaitingRedirect) awaitingRedirect = false;
    }

    private void OnAccount(Account account)
    {
        UIController.Instance.ClosePopup(Popup.StatusPopup);

        // TODO: UIController.Instance.OpenUIWindow(UIWindow.CharacterSelection);
        Debug.Log("Received account with " + account.Characters.Length + " characters");

        // TODO: remove
        admin.RequestRedirect(1, 1);
    }
}
