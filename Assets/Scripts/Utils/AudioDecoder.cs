using System.IO;
using UnityEngine;

namespace Utilities {
    public class AudioDecoder {
        public static byte[] DecodeAudio(AudioClip clip) {
            var samples = new float[clip.samples];
            clip.GetData(samples, 0);

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            int length = samples.Length;
            writer.Write(length);
            foreach (var sample in samples)
                writer.Write(sample);

            return stream.ToArray();
        }
    }
}