using UnityEngine;
using FrameWork.Event;
using Events;

public class ScoreManager : Singleton<ScoreManager>, IEventReceiver<AfterReplyCommentEvent>, IEventReceiver<VscodeScoreEvent>
{
    [Header("Score Model")]
    [Range(0f, 1f)]
    [SerializeField] private float ratingNormalized = 0.75f;
    [SerializeField] private int reviewsCount = 456;

    [Header("Tuning")]
    [SerializeField] private float politeReplyDelta = 0.03f;
    [SerializeField] private float rudeReplyDelta = -0.05f;
    [SerializeField] private int reviewCountDeltaPerAction = 8;

    [Header("Environment")]
    [SerializeField] private int[] gridCount = new int[7];
    [SerializeField] private int[] gridScore = new int[7];


    private int _steamWindowRefCount;
    private bool _pendingUiSync;

    public float RatingNormalized => ratingNormalized;
    public int ReviewsCount => reviewsCount;

    public bool HasSteamWindowRegistered => _steamWindowRefCount > 0;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<AfterReplyCommentEvent>(this);
        EventBus.Subscribe<VscodeScoreEvent>(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<AfterReplyCommentEvent>(this);
        EventBus.Unsubscribe<VscodeScoreEvent>(this);
    }

    public void RegisterSteamWindow()
    {
        _steamWindowRefCount++;
        if (_steamWindowRefCount == 1 && _pendingUiSync)
        {
            _pendingUiSync = false;
            PublishScoreChanged();
        }
    }

    public void UnregisterSteamWindow()
    {
        _steamWindowRefCount = Mathf.Max(0, _steamWindowRefCount - 1);
    }

    public void OnEvent(AfterReplyCommentEvent e)
    {
        if (e.optionIndex == 0)
            ApplyReviewChange(politeReplyDelta, reviewCountDeltaPerAction);
        else if (e.optionIndex == 2)
            ApplyReviewChange(rudeReplyDelta, reviewCountDeltaPerAction);   
    }

    public void OnEvent(VscodeScoreEvent e)
    {
        ApplyVscodeScore(e.counts);
    }

    private void ApplyVscodeScore(int[] counts)
    {
        
    }

    private void ApplyReviewChange(float scoreDelta, int countDelta)
    {
        ratingNormalized = Mathf.Clamp01(ratingNormalized + scoreDelta);
        reviewsCount = Mathf.Max(0, reviewsCount + countDelta);

        if (_steamWindowRefCount > 0)
        {
            _pendingUiSync = false;
            PublishScoreChanged();
        }
        else
            _pendingUiSync = true;
    }

    private void PublishScoreChanged()
    {
        EventBus.Publish(new ScoreChangedEvent(ratingNormalized, reviewsCount));
    }
}
