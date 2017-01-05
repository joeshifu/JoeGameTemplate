using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;


public class AnimationUpdate : MonoBehaviour
{
    public float animationSpeed = 1;
    public int delay = 0;
	public bool setNativeSize = false;
	public bool loop = true; //循环播放标志
	public bool hideOnFinsih = false; //播放完毕后隐藏

    public Sprite[] sprites;
    public int x = 0;//图片变化的位置
    public int y = 0;
    public int z = 0;
    //public GameObject go;
    private Image _image;
    private int spriteAmount;

	private bool m_init = false;
    private Vector3 OldPosition;
    void OnEnable()
    {
		if (m_init == false) {
			_image = GetComponent<Image>();
			spriteAmount = sprites.Length;
			if (delay != 0)
			{
				spriteAmount += delay;
			}
			m_init = true;
		}
        OldPosition = transform.localPosition;

        StopAllCoroutines ();
		StartCoroutine("animate");
    }
    void OnDisable()
    {
        transform.localPosition = OldPosition;
    }
    IEnumerator  animate()
    {
		float offset = 0.1f / animationSpeed;
		int index = 0;
		while (true) {
			_image.sprite = sprites[index];
            transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y + y, transform.localPosition.z + z);

            if (setNativeSize) {
				_image.SetNativeSize ();
			}
			index++;
			if (index >= spriteAmount) {
				if (loop) {
					index = 0;
				} else if (hideOnFinsih) {
					gameObject.SetActive (false);
				}
			}
			yield return new WaitForSeconds (offset);
		}
       
    }
}
