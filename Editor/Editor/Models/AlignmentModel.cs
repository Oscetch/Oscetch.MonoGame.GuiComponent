using Editor.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Editor.Models
{
    public class AlignmentModel
    {
        public int AlignmentDetectionRange { get; }
        public AlignmentType AlignmentType { get; }
        public Vector2 NewPosition { get; private set; }
        public float DistanceToAlignment { get; }
        public bool IsWithinSnapRange { get; }

        public AlignmentModel(Rectangle checkBounds, Rectangle currentBounds, int alignmentDetectionRange)
        {
            AlignmentDetectionRange = alignmentDetectionRange;
            var currentPosition = currentBounds.Location.ToVector2();

            var potentialPositions = CheckForAlignment(currentBounds, checkBounds)
                .OrderBy(x => Vector2.Distance(currentPosition, x))
                .ToList();

            if (potentialPositions.Count != 0)
            {
                NewPosition = potentialPositions.First();
                DistanceToAlignment = Vector2.Distance(currentPosition, NewPosition);
                AlignmentType = GetAlignmentType(NewPosition.ToPoint(), currentPosition.ToPoint());
                IsWithinSnapRange = true;
            }
            else
            {
                IsWithinSnapRange = false;
                DistanceToAlignment = float.MaxValue;
                AlignmentType = AlignmentType.Same;
            }
        }

        private static AlignmentType GetAlignmentType(Point newPosition, Point currentPosition)
        {
            if (newPosition == currentPosition)
            {
                return AlignmentType.Same;
            }

            if (newPosition.X > currentPosition.X)
            {
                return AlignmentType.Right;
            }
            if (newPosition.X < currentPosition.X)
            {
                return AlignmentType.Left;
            }
            if (newPosition.Y > currentPosition.Y)
            {
                return AlignmentType.Below;
            }

            return AlignmentType.Above;
        }

        private IEnumerable<Vector2> CheckForAlignment(Rectangle currentBounds, Rectangle checkBounds)
        {
            #region Y Alignment
            if (IsWithinAlignmentRange(currentBounds.Y, checkBounds.Y, out _))
            {
                yield return new Vector2(currentBounds.X, checkBounds.Y);
            }
            if (IsWithinAlignmentRange(currentBounds.Y, checkBounds.Bottom, out var diff))
            {
                yield return new Vector2(currentBounds.X, currentBounds.Y - diff);
            }
            if (IsWithinAlignmentRange(currentBounds.Y, checkBounds.Center.Y, out diff))
            {
                yield return new Vector2(currentBounds.X, currentBounds.Y - diff);
            }
            if (IsWithinAlignmentRange(currentBounds.Bottom, checkBounds.Bottom, out diff))
            {
                yield return new Vector2(currentBounds.X, currentBounds.Y - diff);
            }
            if (IsWithinAlignmentRange(currentBounds.Bottom, checkBounds.Y, out diff))
            {
                yield return new Vector2(currentBounds.X, currentBounds.Y - diff);
            }
            if (IsWithinAlignmentRange(currentBounds.Bottom, checkBounds.Center.Y, out diff))
            {
                yield return new Vector2(currentBounds.X, currentBounds.Y - diff);
            }
            if (IsWithinAlignmentRange(currentBounds.Center.Y, checkBounds.Center.Y, out diff))
            {
                yield return new Vector2(currentBounds.X, currentBounds.Y - diff);
            }
            #endregion
            #region X Alignment

            if (IsWithinAlignmentRange(currentBounds.X, checkBounds.X, out _))
            {
                yield return new Vector2(checkBounds.X, currentBounds.Y);
            }
            if (IsWithinAlignmentRange(currentBounds.X, checkBounds.Right, out diff))
            {
                yield return new Vector2(currentBounds.X - diff, currentBounds.Y);
            }
            if (IsWithinAlignmentRange(currentBounds.X, checkBounds.Center.X, out diff))
            {
                yield return new Vector2(currentBounds.X - diff, currentBounds.Y);
            }
            if (IsWithinAlignmentRange(currentBounds.Right, checkBounds.Right, out diff))
            {
                yield return new Vector2(currentBounds.X - diff, currentBounds.Y);
            }
            if (IsWithinAlignmentRange(currentBounds.Right, checkBounds.X, out diff))
            {
                yield return new Vector2(currentBounds.X - diff, currentBounds.Y);
            }
            if (IsWithinAlignmentRange(currentBounds.Right, checkBounds.Center.X, out diff))
            {
                yield return new Vector2(currentBounds.X - diff, currentBounds.Y);
            }
            if (IsWithinAlignmentRange(currentBounds.Center.X, checkBounds.Center.X, out diff))
            {
                yield return new Vector2(currentBounds.X - diff, currentBounds.Y);
            }

            #endregion
        }

        private bool IsWithinAlignmentRange(int currentValue, int checkValue, out int diff)
        {
            diff = currentValue - checkValue;
            return Math.Abs(diff) <= AlignmentDetectionRange;
        }
    }
}
