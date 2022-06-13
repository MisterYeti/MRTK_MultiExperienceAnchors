using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoSlide : Slide
{
    [SerializeField] private VideoClip _videoClip;
    private VideoPlayer _videoPlayer = null;

    public VideoClip VideoClip { get => _videoClip; set => _videoClip = value; }


    protected override void Awake()
    {
        base.Awake();
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.clip = _videoClip;
        OnSlideShown.AddListener(PlayVideo);
    }

    public override void Hide(Action onHide, float duration)
    {
        _videoPlayer.Stop();
        base.Hide(onHide, duration);
    }

    public override void Hide(Action onHide)
    {
        _videoPlayer.Stop();
        base.Hide(onHide);
    }

    private void PlayVideo()
    {
        _videoPlayer.loopPointReached += HideVideo;
        _videoPlayer.Play();
    }

    private void HideVideo(VideoPlayer source)
    {
        _videoPlayer.loopPointReached -= HideVideo;
        Hide(null);
    }

}
