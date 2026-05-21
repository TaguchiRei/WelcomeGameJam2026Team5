using UnityEngine;

public class TitleAnimController : MonoBehaviour
{
    private static readonly int AnimNumber = Animator.StringToHash("animNumber");
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        int randomNum = Random.Range(0, 3);
        _animator.SetInteger("AnimNumber", randomNum);
    }
}