/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_ENEMIES_FUSION = 1136450133U;
        static const AkUniqueID PLAY_ENEMIES_MOVEMENT = 2354172050U;
        static const AkUniqueID PLAY_ENEMIES_SHOTS = 1893986092U;
        static const AkUniqueID PLAY_FOOTSTEPS = 3854155799U;
        static const AkUniqueID PLAY_IMPACT_LASER = 2745347784U;
        static const AkUniqueID PLAY_MUSIC = 2932040671U;
        static const AkUniqueID PLAY_PLAYER_DRONE_AIM = 137489100U;
        static const AkUniqueID PLAY_PLAYER_HIT = 1864892471U;
        static const AkUniqueID PLAY_PLAYER_SHOTS = 3890461907U;
        static const AkUniqueID PLAY_PORTAL_IDLE = 686654861U;
        static const AkUniqueID PLAY_STOMP = 2753042117U;
        static const AkUniqueID PLAY_TELEPORTATION = 1187918102U;
        static const AkUniqueID PLAY_TORCH_IDLE = 2909239993U;
        static const AkUniqueID STOP_MUSIC = 2837384057U;
        static const AkUniqueID STOP_PORTAL_IDLE = 3724992451U;
        static const AkUniqueID STOP_TORCH_IDLE = 3950460727U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace PLAYER_LIVES
        {
            static const AkUniqueID GROUP = 2621191170U;

            namespace STATE
            {
                static const AkUniqueID END = 529726532U;
                static const AkUniqueID FULLLIFE = 748970704U;
                static const AkUniqueID LOWLIFE = 19277431U;
                static const AkUniqueID MIDLIFE = 1346429955U;
            } // namespace STATE
        } // namespace PLAYER_LIVES

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID MUSIC_VOLUME = 1006694123U;
        static const AkUniqueID SFX_VOLUME = 1564184899U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID SBK = 460350457U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
