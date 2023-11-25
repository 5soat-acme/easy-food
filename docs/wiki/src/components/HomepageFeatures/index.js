import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

const FeatureList = [
  {
    title: 'Arquitetura de Software',
    Svg: require('@site/static/img/arquitetura.svg').default,
    description: (
      <>
        O projeto é parte do trabalho de conclusão do curso de Pós-Graduação em Arquitetura de Software da FIAP, turma 5SOAT de 2023.
      </>
    ),
  },
  {
    title: 'Projeto',
    Svg: require('@site/static/img/projeto.svg').default,
    description: (
      <>
        Visa a criação de um MVP para autoatendimento de clientes em uma lanchonete.
      </>
    ),
  },
  {
    title: 'Documentação',
    Svg: require('@site/static/img/documentacao.svg').default,
    description: (
      <>
        Focada em documentar o processo de desenvolvimento do projeto, desde a concepção até a entrega.
      </>
    ),
  },
];

function Feature({Svg, title, description}) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center">
        <Svg className={styles.featureSvg} role="img" />
      </div>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures() {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
