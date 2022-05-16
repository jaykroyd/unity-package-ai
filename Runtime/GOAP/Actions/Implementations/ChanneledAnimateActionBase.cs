using System;

namespace Elysium.AI.GOAP
{
    public abstract class ChanneledAnimateActionBase : AnimateActionBase
    {
        private IProgressBar progressBar = default;
        protected IVerifier progressBarFillVerifier = default;

        protected ChanneledAnimateActionBase(IAnimator _animator, IAnimation _animation, IProgressBar _progressBar) : base(_animator, _animation)
        {
            this.progressBar = _progressBar;
            this.progressBarFillVerifier = new Verifier($"{GetType().Name} progress_bar_verifier", TimeSpan.FromSeconds(0.05f));
        }

        public override bool Run(IGoapAgent _agent)
        {
            progressBar.SetActive(true);
            progressBarFillVerifier.Bind(
                () => true, 
                () => progressBar.SetFillAmount(1 - ((float)animator.Elapsed.TotalMilliseconds/(float)animation.Duration.TotalMilliseconds)
            ));
            return base.Run(_agent);
        }

        protected override void OnAnimationEnd(IGoapAgent _agent)
        {
            progressBar.SetActive(false);
            progressBarFillVerifier.Unbind();
            base.OnAnimationEnd(_agent);
        }
    }
}