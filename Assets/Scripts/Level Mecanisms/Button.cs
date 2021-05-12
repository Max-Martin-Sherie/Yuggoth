using UnityEngine;

/// <summary>
/// Items from this class will act like portal buttons "nuff said"
/// </summary>

public class Button : MonoBehaviour
{
    [SerializeField][Tooltip("activator layers")]private LayerMask m_activators;
    
    [SerializeField][Tooltip("the width until which the button will be pressed")]private float m_minimumShrinkWidth = 0.5f;
    [SerializeField][Tooltip("the speed at which the button will be pressed")]private float m_shrinkSpeed = 10f;

    [HideInInspector]public bool m_triggered = false; //the bool that will activate if the button is triggered
    
    
    private float m_startWidth; //the original width of the button
    
    //Initiating the delagate that will be called everytime the GameObject gets hit
    public delegate void OnButtonAction();

    //Creating a delacate call
    public OnButtonAction OnButtonPress;
    
    //Creating a delacate call
    public OnButtonAction OnButtonRelease;
    
    private void Start()
    {
        m_startWidth = transform.localScale.y; //Fetching the original width of the button
    }

    private void Update()
    {
        Vector3 size = transform.lossyScale/2;//Getting the half extends of the button
        if (Physics.BoxCast(transform.position, size, Vector3.up, out RaycastHit hit,transform.rotation,size.y+0.35f,m_activators))
        {
            //Activating the trigger
            if(!m_triggered)
            {
                m_triggered = true;
                if(OnButtonPress !=null) OnButtonPress();
            }
            else
            {
                //animationg the button
                if (transform.localScale.y >= m_minimumShrinkWidth)
                {
                    float y = Mathf.Lerp(transform.localScale.y, m_minimumShrinkWidth, m_shrinkSpeed * Time.deltaTime);
                    transform.localScale = new Vector3(transform.localScale.x, y,transform.localScale.z);
                }
            }
        }
        else
        {
            if (m_triggered && OnButtonPress != null) OnButtonRelease();
            //resseting the trigger
            m_triggered = false;
            transform.localScale = new Vector3(transform.localScale.x, m_startWidth,transform.localScale.z);
        }
    }
}




/*(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((#%%%%%&&&&&&%%%%%%%#####################%%%%%%%%%%%##((////////////////((/((/(((/((((((((((((((((((((((((((((((((((
(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((%%%&%&&&&&&&%%%%######(((((((((((((((((((((#####%%%%%%%#(((//////////////////((///(((((((((((((((((((((((((((((((((((
(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((#%&&&&&&&&&&%%%%######((((((((//////////(((((((((#####%%%##(((////////////////////////(((((((((((((((((((((((((((((((((
((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((%%&&&&&&&&&&%%%%#######((((((((///////////////((((((((((##%###((///////////////////////(((((((((((((((((((((((((((((((((
(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((%%&&&&&&&&&&&%%%########(((((((((////////////////((((((((((##%##((//////////////////////(((((((((((((((((((((((((((((((((
((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((%%&&&&&&&&&&%%%#########((((((((((//////////////////((((#####%%%#(((//////////////////////((((((((((((((((((((((((((((((((
(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((#%&&&&&&&&&&%%#########(((((((((((/////////////////////(((###%%%%%((((////////////////////////(((((((((((((((((((((((((((((
((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((#%&&&&&@@@&&%##########((((((((((((////////////////////////(##%&&&&%#((//////////////////////((/((((((((((((((((((((((((((((
#####(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((#&&&&@@@@@&%%%%%%%%%%%%######(((((((((((////////////////////((#%&&&&#(////////////////////////(((//(((((((((((((((((((((((((
#######((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((#%&&&@@@@@@@&&&&&&&&&&&&&&&&%%%###((((((((((((((#######((((///(#%&&&&&#(///////////////////////(((/((((((((((((((((((((((((((
#############(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((#%&&&@@@@@@@@@&&&&&%%%%%%%%%&&&&&&&%%########%%%&&%%%%%%%%###(((#%&&&&&%((/////////////////////////(((((((((((((((((((((((((((
################((((((((((((((((((((((((((((((((((((((((((((((((((((((((((%&&@@@@@@@&&&&%%%&&&&&&&&&&%&&&&&&&&&&&&&&&%%%%%%%#########%%&&&&&&&&&&%((////////////////////////////((((((((((((((((((((((((
######################((((((((((((((((((((((((((((((((((((((((((((((((((((%&@@@@@@&&%%&&%%%%%&&&&&&&&&%%%%%%&&&&&&&%###%%%%%%&%%%%%%%###%&@@@&&&&&(/////////////////////////////((((((((((((((((((((((((
##########################((((((((((((((((((((((((((((((((((((((((((((((((%@@@@@@@&%%%%&%###%%%%%%%%%%%%%%%%&&%###%%#(##%%%%%&&&&&&%%%###%&#%&&&%%(//////////////////////////////(((((((((((((((((((((((
##########################((((((((((((((((((((((((((((((((((((((((((((((((#&@@@@@@&%%%%%%%###############%&&%%#((((%%(((####%%%%%%##(((((%#((%&%#((///////////////////////////////((((((((((((((((((((((
############################(((((((((((((((((((((((((((((((((((((((((((((((%&@@@@&%%%%%%%%%%%##########%%&&%%#((((((##((/(((((##((((((((%#//(#%#(((////////////////////////////////(((((((((((((((((((((
###############################(((((((((((((((((((((((((((((((((((((((((((((%@@@@&%%%%%%##%%%######%%%%%%%%%##(((////(%##(((((((((((((##((//(##(((((///////////////////////////////(((((((((((((((((((((
#################################(((((((((((((((((((((((((((((((((((((((((((#&@@@&%%%##########((######%%%%%%%###((((((#%%%%########(((/////(##((((////////////////////////////////(((((((((((((((((((((
###################################((((((((((((((((((((((((((((((((((((((((((#%&&&%%%%################%%%%%%&%%%##%%#(((###((((((///////////(##((((////////////////////////////////(((((((((((((((((((((
#################################(((((((((((((((((((((((((((((((((((((((((((((##%%%%%%%################%%%%%%%%##((###(((#####((((/////////(##(((////////////////////////////////(((((((((((((((((((((((
####################################(((((((((((((((((((((((((((((((((((((((((((((##%%%%%%%####################(((((//////((#####(((////(/(((#(((/////////////////////////////////(((((((((((((((((((((((
######################################((((((((((((((((((((((((((((((((((((((((((((#%%%%%%%%##########%%%%%%%######((//////(##%###(((((((((((#(((/////////////////////////////(((((((((((((((((((((((((((
########################################(((((((((((((((((((((((((((((((((((((((((((%%%%%%%%%%####%%%%&&&&&&&%%%%%%%%%%#((/((##%###(((((((((##(((////////////////////////////(///((((((((((((((((((((((((
############################################(((((((((((((((((((((((((((((((######%%%%%%%%%######%%&&&&&&&%%%########%%&&%%#######(((((((((#%##((((((((((/(((///////////////(((((((((((((((((((((((((((((
###############################################(#(((((((((((((########%%%%%&&&&&&&&&%%%%%#######%%%&&&&&%%%%#########%%%%######((((((((((#%%%##(((((((((((((((((((((((((((((((((((((((((((((((((((((((((
########################################################%%%%%%%%%%&&&&&@@@@@&&&&&&&%%%%%%%########%%%%&&&%%%%%%%%%#####((///((((((((((((#%&&&&&&&&&%%##((((((((((((((((((((/((((((((((((((((((((((((((((
###########################################%%%%%%%%%%&&&&&&&&@@@@@@@@@@&&&&&&%%%%%%%%%%%%%%%########%%%%%%%%%%%%%###((((/////(((((((((((((((#%&@@@@@@@@@@@@@@&&#((((((((((((((((((((((((((((((((((((((((
######################################%%%%%&&&&&&@@@@@@@@@@@@@@@@@@@@&&%%%%%%%%%%%%%%%%%%%%%%%##############((((((((/////////(((((((((((((((((((%&@@@@@@@@@@@@@@@@@@@&#(((((((((((((((((((((((((((((((((
#################################%%%%%&&&&&@@@@@@@@@@@@@@@@@@@@@@@@@&&%#####%%%%%%%%%%%%%%%%%%%%%#######((((((//////////////((((((((((((((///(((#%&&@@@@@@@@@@@@@@@@@@@@@@@&%#((((((((((((((((((((((((((
#############################%%%%%%&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&%######%%%%%%%%%%%%%%%%%%%%%%%######((((((//////////(((((((((((//(((///((((##%&@@@@@@@@@@@@@@@@@@@@@@@@@@@@&%((((((((((((((((((((((
#####%%%%###############%%%%%%&&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&%#######%%%%%%%%%%%%%%%%%%%%%%%%#####((((((((////(((((((((((((///////(((((((##%&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&#((((((((((((((((((
%%%%%%%%##%%%%%%%%%%%%%%%%%&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&%########%%%%%%%%%%%%%%%%%%%%%%%%%%%######((((((((((((((((((((/////(((((((((((#&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%(((((((((((((((
%%%%%%%%%%%%%%%%%%%%%%%&&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&%##(#(####%%%%%%%%%%%%%%%%%%%%%%%%%%%%%######((((((((((((((((((/////((((((((((#%&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%#(((((((((((
%%%%%%%%%%%%%%%%%%%&&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&%#(#(#((###%%%%%%%%%%%%%%%%###################(((((((((((((((((/////(((((((((##&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&#((((((((
###################%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%########(((((((((((((((((((((((((((#(((((((((/((((((((((((((((((((((///////////(((((((#########%%%#%##%%#%####%#######################(((((
#%%%%%%%%%%##%#%%%%####%%%%%#%%%%%###(((((((##%%%%%%%%%%%%%%%#((((///((##%%%%%%%%###(((/////((#%%%%%%%%%###(((((#%%%%%%%%%%%%%%%#((###%%%%%%%%%%%%##((////((#%%%%%%%%%%%%%%%######%%%%%%%%%%%%##((//////
%%%%%&&&&&&&&%%%%%%%##%&&&&&&&&&&&&&%%##(((###%%%%&&&&&&%%%%%##((((#%%%&&&&&&%%%%%%%%######%%&&&&%%%%%%%%%%#####%%&&%%%%%%%%#%###((##%%&&&%%%%%%%&&%%#((((((#%%&%%%%#######((((##%%%%%%%%%%%%&&&%%#((//(
//(((#%%&&%#((((////((#%%%%######%%%%%%#((((/////(#%%%%#((((////(##%%&%%%##(((///(((((((#%%%%%%##((((////((((((##%%##((////********(##%&%%#((((#%%%%%%#((((##%%%##(////*******((##%%#(((//(((#%%%%%#((((
///(((#%&&%#((((////((#%%%%#((####%%%%%#((((////((#%&&%##((((((((#%%%%#(((((/////////((#%%&%%#(((((///////////(##%%%##(((/(///**////(#%&%%#((((##%%%%%#((((##%%%##(((((///////((##%%#((//////((#%%%%#(((
///((#%%&%%#((((////((#%&&%###%%%%%%%%#(((//////((#%%&%##((((((((%%&%%#####%%%%&%%%%%%%%%&%%#####%%%%&%%%%%####%%%&&&&&&&&&&&%%##((##%%&&&%%%%%%%%%%%#(((((##%%&&&&&&&&&&%%%#####%%%#((///////(#%%%%#(((
////((#%&%%#((((///((##%&&&&&&&&&&&%##(((////////(#%%&%##(((((((#%&&%##(((####%%%&&%%%%%&&%%##(#######%%&%%#####%%%%%%#######((((((((#%&&&&&&&&&&%%##(////((#%%%%%#####(((((((((#%%%%#((/////((#%%%%#(((
////(##%&%%#((((///((##%&&&%%%%&%%%%#(((////////((#%%%%##((((((((#%%%%#((((//((##%%%%%%%%&&%#((((///((#%%%######%%%%#((////*******//(#%%%%%%%%%%%%%#(//////(##%%##((/////****/(((#%%%#(//////((#%%%##(((
////(##%&%%#((((///(((#%&&%####%%%%%%#((((///////(#%%%%#(((((///(#%%%%%##((/((((#%%%%##%%%&%%##((((((##%%%%#((##%%%%#((/////*****//((#%&%%##(##%%%%%#(((((((#%%%#((/////****////(#%%##(//((((#%%&%%#((((
////((#%&&%#((((///(((#%%%%#((####%%%%%#((((((((#%%&&&&%%####(((((##%%&%%%%%%%%%%&%%%####%%&&&%%%%%#%%%&%%%#####%%%&%%%######(((((((#%%&%%#(((###%%%%%#(((###%%%%%%%########(((#%%%&&%%%%%%%&&&%%%#(////