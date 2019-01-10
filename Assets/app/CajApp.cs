using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CajApp : MonoBehaviour {
    //========================================
    public enum ActionMode {
        ACTION_MODE_EASY,
        ACTION_MODE_CLASSIC
    };
    private readonly string[] aActionModeNames = new string[] { "Easy", "Classic" };
    //========================================    
    static public CajApp instance;
    static public ActionMode actionMode = ActionMode.ACTION_MODE_EASY;
    //========================================
    //#pragma warning disable 0649
    [SerializeField] public GameObject player1 = null;
    [SerializeField] private GameObject actionCanvas = null;    
    [SerializeField] private GameObject ball = null;
    [SerializeField] private GameObject blockWhite = null;
    [Space]
    [SerializeField] private GameObject resultsWindow = null;
    [SerializeField] private Text resultsText = null;
    [Space]
    [SerializeField] private AudioSource hitSound = null;
    [SerializeField] private AudioSource winSound = null;
    [SerializeField] private AudioSource gameOverSound = null;
    //#pragma warning restore 0649
    //========================================
    [HideInInspector] public float appWidth = 1920;
    [HideInInspector] public float appHeight = 1080;

    private bool isRound = true;
    private List<GameObject> aBlocks;
    private System.Random rnd;

    private float actionRoundTime = 0.0f;
    private int totalBlocks = 0;
    private int destroyedBlocks = 0;
    //========================================
    private void Awake() {
        instance = this;

        CameraAspectKeeper cameraController = FindObjectOfType<CameraAspectKeeper>();
        if (cameraController != null) {
            cameraController.setResizeListener(this.onResize);
        }
    }
    //========================================
    void Start() {        
        init();
        createBlocks();
    }
    //========================================
    public void trace(System.Object msg) {
        UnityEngine.Debug.Log(msg.ToString());
    }
    //========================================
    private void init() {
        rnd = new System.Random();

        var rect = actionCanvas.GetComponent<RectTransform>().rect;
        appWidth = rect.width;
        appHeight = rect.height;
        Debug.Log("appSize = " + appWidth + "x" + appHeight);
        Debug.Log("sound = " + SoundEnabler.isSoundEnabled());

        resultsWindow.SetActive(false);
    }
    //========================================
    private void createBlocks() {
        const float borderW = 5.0f;

        //game design values:                
        float blockGapX, blockGapY;
        int blocksInRow;
        Color32[] aColors;
        if (actionMode == ActionMode.ACTION_MODE_EASY) {
            blockGapX = rnd.Next(5, 25);
            blockGapY = rnd.Next(10, 15);
            blocksInRow = 1 + (2 * rnd.Next(1, 3));
            aColors = new Color32[] {
                new Color32(255, 0, 0, 255), new Color32(0, 255, 0, 255), new Color32(32, 32, 255, 255)
            };
        } else {
            blockGapX = 5.0f;
            blockGapY = 5.0f;
            blocksInRow = 17;
            aColors = new Color32[] {
                new Color32(255, 0, 0, 255), new Color32(0, 255, 0, 255), new Color32(32, 32, 255, 255),
                new Color32(255, 255, 255, 255),
                new Color32(0, 255, 255, 255), new Color32(255, 255, 0, 255)
            };
        }

        //calc start block position:
        blockWhite.SetActive(true);
        float y = blockWhite.transform.position.y + rnd.Next(-10, 10);
        float z = blockWhite.transform.position.z;
		var bsize = blockWhite.GetComponent<BoxCollider2D>().bounds.size;
        float w = bsize.x;
        float h = bsize.y;

        float dx = w + blockGapX;
        float dy = h + blockGapY;

        float actionW = appWidth - borderW * 2.0f;
        float blocksW = blocksInRow * dx - blockGapX;
        float startX = (actionW - blocksW) / 2;
        startX = startX - appWidth / 2 + w / 2; //center coords are {0; 0}

        //create new blocks - clone blockWhite:
        aBlocks = new List<GameObject>();
        for (int i = 0; i < aColors.Length; i++) {
            float x = startX;
            for (int j = 0; j < blocksInRow; j++) {
                var block = Instantiate(blockWhite, new Vector3(x, y, z), Quaternion.identity);
                block.GetComponent<SpriteRenderer>().color = aColors[i];
                aBlocks.Add(block);
                x += dx;
            }
            y -= dy;
        }
        totalBlocks = aBlocks.Count;
        destroyedBlocks = 0;

        //hide original block:
        blockWhite.SetActive(false);
    }
    //========================================
    private void destroyBlocks() {
        aBlocks.ForEach(b => {
            if (b != null) {
                Destroy(b);
            }
        });
    }
    //========================================
    public void restartAction() {
        //reset blocks:
        destroyBlocks();
        createBlocks();

        //center p1:        
        var pos = player1.transform.position;
        pos.x = 0;
        player1.transform.position = pos;
        player1.SetActive(true);

        //reset ball:        
        pos = ball.transform.position;
        pos.x = 0;
        pos.y = 0;
        ball.transform.position = pos;
        ball.SetActive(true);
        ball.GetComponent<Ball>().restart();

        //stats:
        actionRoundTime = 0.0f;

        //hide windows:
        resultsWindow.SetActive(false);

        isRound = true;
    }
    //========================================    
    public void onResize() {
        //createScreenWalls();
    }
    //========================================
    private List<UnityEngine.Object> aScreenWalls = new List<UnityEngine.Object>();
    private void createScreenWalls() {
        //add screen walls for misc screen aspects:
        //works fine but looks bad without good textures. nn:
        /*
        float screenAspect = ((float)Screen.width / Screen.height);
        float appAspect = appWidth / appHeight;
        //too lazy to make fields, rare calls, so works fine:
        var sw = GameObject.Find("screenWalls");
        var wallTemp = GameObject.Find("wallScreen");
        if (sw == null || wallTemp == null) {
            Debug.Log("CajApp.onResize: can't find screen walls");
            return;
        }
        //remove old:
        foreach (var obj in aScreenWalls) {
            Destroy(obj);
        }
        aScreenWalls = new List<UnityEngine.Object>();
        //add new walls to bg layer:        
        //(no CameraAspectKeeper for that layer, so use appWidth & appHeight as Screen sizes)
        if (Math.Abs(screenAspect - appAspect) > 0.1f) { //no 0.001 (UnityEditor screen scale).
            if (screenAspect > appAspect) {
                //>: left & right spaces: ok, nothing (?).
            } else {
                //<: top & bottom spaces:
                float size = 5.0f / (appAspect / screenAspect);
                var q = Quaternion.identity;
                //top wall:
                var ww = Instantiate(wallTemp, new Vector3(0, appHeight / 2 - size / 2, 1), q);
                ww.transform.localScale = new Vector3(appWidth / 2, size, 1);
                aScreenWalls.Add(ww);
                //right wall:
                float x = appWidth / (appAspect / screenAspect) / 2 - size / 2;
                ww = Instantiate(wallTemp, new Vector3(-x, 0, 1), q);
                ww.transform.localScale = new Vector3(size, appHeight / 2, 1);
                aScreenWalls.Add(ww);
                //left wall:
                ww = Instantiate(wallTemp, new Vector3(x, 0, 1), q);
                ww.transform.localScale = new Vector3(size, appHeight / 2, 1);
                aScreenWalls.Add(ww);
            }
        }
        */
    }
    //========================================
    public void onGameOver() {
        Debug.Log("game over");
        resultsText.text = "Game Over." +
            "\r\nGoals = " + destroyedBlocks + " / " + totalBlocks;
        if (SoundEnabler.isSoundEnabled()) {
            gameOverSound.Play();
        }        
        Invoke("showResultsWindow", 0.25f); //reaction delay - realize what's happened.
    }
    //========================================
    private void onWin() {
        Debug.Log("win");

        int score = (int)actionRoundTime;
        int bestScore = getBestScore();
        if (score < bestScore || bestScore == 0) {
            setBestScore(score);
        }

        resultsText.text = "Win!";
        showResultsWindow();
    }
    //========================================
    private void setBestScore(int score) {
        PlayerPrefs.SetInt("Arkanoid_" + aActionModeNames[(int)actionMode] + "_bestTime", score);
    }
    //========================================
    private int getBestScore() {
        return PlayerPrefs.GetInt("Arkanoid_" + aActionModeNames[(int)actionMode] + "_bestTime", 0);
    }
    //========================================
    private void showResultsWindow() {
        resultsText.text += "\r\nTotal time = " + (int)actionRoundTime + " sec.";

        int bestTime = getBestScore();
        if (bestTime > 0) {
            resultsText.text += "\r\n\r\n\r\nBest Time [" + 
                                aActionModeNames[(int)actionMode] + "] = " + bestTime.ToString() + ".";
        }

        resultsWindow.SetActive(true);
        ball.SetActive(false);
        player1.SetActive(false);
        isRound = false;
    }
    //========================================
    public void onBlockDestroy(Collision2D block) {
        destroyedBlocks++;
        if (destroyedBlocks < totalBlocks) {
            playHitSound(0.5f);
            //TODO: particle effects, show counter or smth else.            
        } else {
            if (SoundEnabler.isSoundEnabled()) {
                winSound.Play();
            }            
            onWin();            
        }
    }
    //========================================
    public void playHitSound(float pitch) {
        hitSound.pitch = pitch;
        if (SoundEnabler.isSoundEnabled()) {
            hitSound.Play();
        }
    }
    //========================================
    private void checkInput() {
        //optional hotkeys:
        if (Input.GetKeyDown(UnityEngine.KeyCode.Return)) {
            if (!isRound) {
                restartAction();
            }
        } else if (Input.GetKeyDown(UnityEngine.KeyCode.Escape)) {
            CajScenes.showMainMenu();
        }
    }
    //========================================
    void Update() {
        checkInput();

        if (!isRound) return;
        actionRoundTime += Time.deltaTime;
    }
    //========================================
}
