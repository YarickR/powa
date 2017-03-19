public static class PConst
{
	public const int Side_Invalid 	= 0;
	public const int Side_User 		= 1;
	public const int Side_Enemy 	= 2;
	public const int VType_Invalid 	= 0; //Vehicle type
	public const int VType_Light 	= 1;
	public const int VType_LightRanged = 2;
	public const int VType_Medium 	= 3;
	public const int VType_MediumRanged = 4;
	public const int TType_Base 	= 0; // Terrain type
	public const int TType_Desert 	= 1;
	public const int TType_Dirt 	= 2;
	public const int TType_Forest 	= 3;
	public const int TType_Hill 	= 4;
	public const int TType_Marsh 	= 5;
	public const int TType_Mountain = 6;
	public const int TType_Plain 	= 7;
	public const int TType_Void 	= 8;
	public const int TType_Water 	= 9;
	public const int BType_Howitzer = 1;
	public const int BType_Cannon 	= 2;
	public const int Max_Players    = 2;
	public const int Map_Size		= 12;
	public const int Move_Time		= 13;
	public const int EOT_None 		= 0;
	public const int EOT_Soft 		= 1;
	public const int EOT_Hard 		= 2;
	public const string Server_URL  = "http://powa.ext.terrhq.ru";
	public const string LPURL 		= "/powa.poll";
}

public static class PCmds
{
   	public const int START_FIGHT     = 1;
	public const int END_FIGHT       = 2;
	public const int VEHICLE_POS     = 3;
	public const int VEHICLE_MOVE    = 4;
	public const int VEHICLE_ATTACK  = 5;
	public const int SETUP_FIELD  	 = 6;
	public const int PASS_MOVE  	 = 7;
	public const int END_TURN		 = 8;
	public const int NEXT_TURN		 = 9;
}


public static class PCmdState {
	public const int READY 		= 1;
	public const int PROCESSING = 2;
	public const int DONE		= 3;
}
public static class PCmdFlags {
	public const int DEFAULT	= 0;
	public const int PARALLEL	= 1;
}

public static class FIGHT_STATE
{
	public const int LOBBY		 = 1;
	public const int PREPARING   = 2;
	public const int READY       = 3;
	public const int RUNNING     = 4;
	public const int FINISHED    = 5;
}

