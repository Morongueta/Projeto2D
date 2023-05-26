using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperDestroyer : MonoBehaviour
{
    [SerializeField]private AudioClip shredderSound;
    [SerializeField]private ParticleSystem paperParticle;

    [SerializeField]private Vector3 startDestroyingPos;
    [SerializeField]private Vector3 endDestroyingPos;


    public void DestroyPaper(GameObject paperObj)
    {
        #region Setting Particle Color
        var partMain = paperParticle.main;
        Color partCol = Color.white;

        SpriteRenderer paperRender = paperObj.GetComponentInChildren<SpriteRenderer>();

        if(paperRender != null)
        {
            partCol = paperRender.sprite.texture.GetPixel(6,37);
        }
        
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient(partCol);
        partMain.startColor = color;
        #endregion

        paperParticle.Play();

        paperObj.transform.position = startDestroyingPos + transform.position;
        paperObj.GetComponent<Paper>().canGet = false;

        GameObject sfx = SoundManager.Instance.PlaySound(shredderSound, .15f, false);
        paperObj.LeanMove(endDestroyingPos + transform.position, 1f).setOnComplete(()=>{
            Destroy(paperObj);
            Destroy(sfx);
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startDestroyingPos + transform.position, .1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endDestroyingPos + transform.position, .1f);
    }
}
