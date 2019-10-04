namespace HammingCodeCSharp
{
   using System;
   class Program
   {
      static void Main( string[ ] aoArgs )
      {
         bool kbRun = true;
         string    koLine = "";
         string[ ] koArgs;

         MUsage( );

         while( kbRun )
         {
            koLine = Console.ReadLine( );
            koArgs = koLine.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
            
            if( koArgs.Length > 0 )
            {
               switch( koArgs[ 0 ] )
               {
                  case "h":
                  case "ham":
                  {
                     int kiCode = MHam( koArgs[ 1 ][ 0 ] );
                     Console.WriteLine( "Code: " );
                     for( int i = 10; i >= 0; i-- )
                     {
                        Console.Write( ( kiCode & ( 0x00000001 << i ) ) >> i );
                     }
                     Console.WriteLine( );
                     break;
                  }
                  case "d":
                  case "deham":
                  {
                     int kiCode = Convert.ToInt32( koArgs[ 1 ], 2 );
                     char kcChar = MDeham( kiCode );
                     Console.WriteLine( "Character: " + kcChar );
                     break;
                  }
                  default:
                  {
                     MUsage( );
                     break;
                  }
               }
            }
            else
            {
               MUsage( );
            }
         }
      }

      static void MUsage( )
      {
         Console.WriteLine( "Usage:" );
         Console.WriteLine( "   h <c>        Outputs the hamming code for given character, c" );
         Console.WriteLine( "   d <c>        Outputs the character for the given hamming code, c" );
         Console.WriteLine( "   b <s>        Input string, s, is hammed, then de-hammed and printed" );
      }

      static int MHam( char acChar )
      {
         int[ ] kiBit = new int[ 11 ];
         int    kiCode = 0;

         kiBit[ 2  ] = ( ( acChar & 0x01 ) == 0 ) ? 0 : 1;
         kiBit[ 4  ] = ( ( acChar & 0x02 ) == 0 ) ? 0 : 1;
         kiBit[ 5  ] = ( ( acChar & 0x04 ) == 0 ) ? 0 : 1;
         kiBit[ 6  ] = ( ( acChar & 0x08 ) == 0 ) ? 0 : 1;
         kiBit[ 8  ] = ( ( acChar & 0x10 ) == 0 ) ? 0 : 1;
         kiBit[ 9  ] = ( ( acChar & 0x20 ) == 0 ) ? 0 : 1;
         kiBit[ 10 ] = ( ( acChar & 0x40 ) == 0 ) ? 0 : 1;

         kiBit[ 0 ] = kiBit[ 2 ] ^ kiBit[ 4 ] ^ kiBit[  6 ] ^ kiBit[ 8 ] ^ kiBit[ 10 ];
         kiBit[ 1 ] = kiBit[ 2 ] ^ kiBit[ 5 ] ^ kiBit[  6 ] ^ kiBit[ 9 ] ^ kiBit[ 10 ];
         kiBit[ 3 ] = kiBit[ 4 ] ^ kiBit[ 5 ] ^ kiBit[  6 ];
         kiBit[ 7 ] = kiBit[ 8 ] ^ kiBit[ 9 ] ^ kiBit[ 10 ];

         // 1 2 3 4 5 6 7 8 9 A B
         // 0 1 2 3 4 5 6 7 8 9 A
         // C C D C D D D C D D D
         for( int i = 0; i < 11; i++ )
         {
            kiCode |= ( kiBit[ i ] << i );
         }

         return( kiCode );
      }

      static char MDeham( int aiCode )
      {
         char   kcChar  = '\0';
         int[ ] kiBit   = new int[ 11 ];
         int    kiCount = 0;

         for( int i = 0; i < 11; i++ )
         {
            kiBit[ i ] = ( aiCode & ( 0x00000001 << i ) ) >> i;
         }

         if( kiBit[ 0 ] != ( kiBit[ 2 ] ^ kiBit[ 4 ] ^ kiBit[ 6 ] ^ kiBit[ 8 ] ^ kiBit[ 10 ] ) )
         {
            kiCount += 1;
         }

         if( kiBit[ 1 ] != ( kiBit[ 2 ] ^ kiBit[ 5 ] ^ kiBit[  6 ] ^ kiBit[ 9 ] ^ kiBit[ 10 ] ) )
         {
            kiCount += 2;
         }

         if( kiBit[ 3 ] != ( kiBit[ 4 ] ^ kiBit[ 5 ] ^ kiBit[  6 ] ) )
         {
            kiCount += 4;
         }

         if( kiBit[ 7 ] != ( kiBit[ 8 ] ^ kiBit[ 9 ] ^ kiBit[ 10 ] ) )
         {
            kiCount += 8;
         }

         // If valid
         if( ( kiCount > 0 ) && ( kiCount < 12 ) )
         {
            kiBit[ kiCount ] = ( kiBit[ kiCount ] == 0 ) ? 1 : 0;
         }

         kcChar = ( char )( ( kiBit[ 2 ] << 0 ) | ( kiBit[ 4 ] << 1 ) | ( kiBit[ 5 ] << 2 ) | ( kiBit[ 6 ] << 3 ) |
                            ( kiBit[ 8 ] << 4 ) | ( kiBit[ 9 ] << 5 ) | ( kiBit[ 10 ] << 6 ) );

         return( kcChar );
      }
   }
}
