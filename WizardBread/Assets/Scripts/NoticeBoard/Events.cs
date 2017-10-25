using UnityEngine;
using System.Collections;

/* ####################################### */
//                                         //
//         GLO_ = Global Events            //
//         LEV_ = Level Events             //
//         LIQ_ = Liquid Events            //
//                                         //
/* ####################################### */

public class Events : MonoBehaviour
{
	public enum Event
    {
        GLO_EnterPlay,
        GLO_EnterMenu,
        GLO_PlayerWon,
        GLO_PlayerDied,
        LEV_TransitionSectionStart,
        LEV_TransitionSectionComplete,
        LIQ_ChangeCreepingWait,
        LIQ_ChangeRealisiticWait,
        Count
    }
}
